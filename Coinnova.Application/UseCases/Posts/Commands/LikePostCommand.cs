using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Posts.Commands;

public record LikePostCommand(int PostId) : IRequest<PostsForUserIdResponseDto?>;

public class LikePostCommandHandler : IRequestHandler<LikePostCommand, PostsForUserIdResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public LikePostCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PostsForUserIdResponseDto?> Handle(LikePostCommand request, CancellationToken cancellationToken)
    {
        var updatedPost = await _unitOfWork.Posts.LikePostById(request.PostId);
        if (updatedPost == null)
            return null;

        await _unitOfWork.Complete();
        return updatedPost.Adapt<PostsForUserIdResponseDto>();
    }
} 