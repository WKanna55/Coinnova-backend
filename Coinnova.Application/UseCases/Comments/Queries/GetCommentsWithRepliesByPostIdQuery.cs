using Coinnova.Application.Dtos.Comment;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Comments.Queries;

public class GetCommentsWithRepliesByPostIdQuery : IRequest<IEnumerable<CommentWithRepliesDto>>
{
    public int PostId { get; set; }
    public int? RequestDepth { get; set; }
}

internal sealed class GetCommentsWithRepliesByPostIdQueryHandler : IRequestHandler<GetCommentsWithRepliesByPostIdQuery, IEnumerable<CommentWithRepliesDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private const int DefaultRequestedDepth = 3;
    private const int MaxRepliesToLoadThreshold = 10;

    public GetCommentsWithRepliesByPostIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CommentWithRepliesDto>> Handle(GetCommentsWithRepliesByPostIdQuery request, CancellationToken cancellationToken)
    {
        var targetDepth = request.RequestDepth ?? DefaultRequestedDepth;
        var rootComments = await _unitOfWork.Comments.GetRootCommentsAsync(request.PostId);

        var resultDtos = new List<CommentWithRepliesDto>();

        foreach (var comment in rootComments)
        {
            var dto = await BuildCommentWithRepliesAsync(comment, 0, targetDepth);
            resultDtos.Add(dto);
        }

        return resultDtos;
    }

    private async Task<CommentWithRepliesDto> BuildCommentWithRepliesAsync(Comment comment, int currentDepth, int targetDepth)
    {
        var dto = comment.Adapt<CommentWithRepliesDto>();

        if (currentDepth < targetDepth && comment.ReplyCount > 0 && comment.ReplyCount <= MaxRepliesToLoadThreshold)
        {
            var replies = await _unitOfWork.Comments.GetRepliesAsync(comment.Id);
            foreach (var reply in replies)
            {
                var replyDto = await BuildCommentWithRepliesAsync(reply, currentDepth + 1, targetDepth);
                dto.Replies.Add(replyDto);
            }
        }

        return dto;
    }
}