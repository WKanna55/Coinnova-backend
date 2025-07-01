using Coinnova.Application.Dtos.Comment;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Comments.Queries;

public class GetCommentRepliesQuery : IRequest<IEnumerable<CommentWithRepliesDto>>
{
    public int CommentId { get; set; }
    public int? RequestDepth { get; set; }
}

internal sealed class GetCommentRepliesQueryHandler : IRequestHandler<GetCommentRepliesQuery, IEnumerable<CommentWithRepliesDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private const int DefaultRequestedDepth = 3;
    private const int MaxRepliesToLoadThreshold = 10;

    public GetCommentRepliesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CommentWithRepliesDto>> Handle(GetCommentRepliesQuery request, CancellationToken cancellationToken)
    {
        var targetDepth = request.RequestDepth ?? DefaultRequestedDepth;
        var comment = await _unitOfWork.Comments.GetCommentById(request.CommentId);

        if (comment is null)
            return Enumerable.Empty<CommentWithRepliesDto>();

        var commentDepth = await _unitOfWork.Comments.GetCommentDepthAsync(comment.Id);
        var replies = await _unitOfWork.Comments.GetRepliesAsync(comment.Id);

        var resultDtos = new List<CommentWithRepliesDto>();
        foreach (var reply in replies)
        {
            var dto = await BuildCommentWithRepliesAsync(reply, commentDepth, targetDepth);
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