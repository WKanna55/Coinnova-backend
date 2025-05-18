using Coinnova.Application.Dtos.Comment;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;

namespace Coinnova.Application.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    
    private const int DefaultRequestedDepth = 3; // Profundidad si el cliente no especifica (raíz + 1 nivel de respuestas)
    private const int MaxRepliesToLoadThreshold = 10; // Si un comentario tiene más respuestas que esto, no se cargan sus hijos en este DTO.

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CommentWithRepliesDto>> GetCommentsWithRepliesByPostIdAsync(int postId, int? requestDepth = null)
    {
        var targetDepth = requestDepth ?? DefaultRequestedDepth;
        var rootComments = await _unitOfWork.Comments.GetRootCommentsAsync(postId);

        var resultDtos = new List<CommentWithRepliesDto>();

        foreach (var comment in rootComments)
        {
            var dto = await BuildCommentWithRepliesAsync(comment, 0, targetDepth);
            resultDtos.Add(dto);
        }

        return resultDtos;
    }

    private async Task<CommentWithRepliesDto> BuildCommentWithRepliesAsync(
        Comment comment,
        int currentDepthLevel,
        int targetDepth)
    {
        var dto = comment.Adapt<CommentWithRepliesDto>();

        if (currentDepthLevel < targetDepth && comment.ReplyCount > 0 && comment.ReplyCount <= MaxRepliesToLoadThreshold)
        {
            var replies = await _unitOfWork.Comments.GetRepliesAsync(comment.Id);
            
            foreach (var reply in replies) // repliesData ya está ordenado por CreatedAt en el repositorio
            {
                var replyDto = await BuildCommentWithRepliesAsync(
                    reply,
                    currentDepthLevel + 1, 
                    targetDepth
                );
                dto.Replies.Add(replyDto); // Add individual DTO a la lista
            }
        }
        
        return dto;
    }
}