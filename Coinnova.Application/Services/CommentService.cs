using Coinnova.Application.Dtos.Comment;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;

namespace Coinnova.Application.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    
    private const int DefaultRequestedDepth = 1; // Profundidad si el cliente no especifica (raíz + 1 nivel de respuestas)
    private const int MaxAllowedDepth = 3;       // Máxima profundidad que el backend procesará para evitar abusos.
    private const int MaxRepliesToLoadThreshold = 10; // Si un comentario tiene más respuestas que esto, no se cargan sus hijos en este DTO.

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CommentWithRepliesDto>> GetCommentsWithRepliesByPostIdAsync(int postId, int? requestDepth = null)
    {
        var effectiveDepth = Math.Min(requestDepth ?? DefaultRequestedDepth, MaxAllowedDepth);
        var rootCommentsData = await _unitOfWork.Comments.GetRootCommentsWithReplyCountAsync(postId);

        var resultDtos = new List<CommentWithRepliesDto>();

        foreach (var rootData in rootCommentsData)
        {
            var dto = await BuildCommentWithRepliesAsync(rootData.Comment, rootData.ReplyCount, 0, effectiveDepth);
            resultDtos.Add(dto);
        }

        return resultDtos;
    }
    
    private async Task<CommentWithRepliesDto> BuildCommentWithRepliesAsync(
        Comment comment,
        int replyCountForThisComment,
        int currentDepthLevel,
        int targetDepth)
    {
        var dto = comment.Adapt<CommentWithRepliesDto>();

        dto.RepliesCount = replyCountForThisComment;

        if (currentDepthLevel < targetDepth && replyCountForThisComment > 0 && replyCountForThisComment <= MaxRepliesToLoadThreshold)
        {
            var repliesData = await _unitOfWork.Comments.GetRepliesWithReplyCountAsync(comment.Id);
            
            foreach (var replyData in repliesData) // repliesData ya está ordenado por CreatedAt en el repositorio
            {
                var replyDto = await BuildCommentWithRepliesAsync(replyData.Comment, replyData.ReplyCount, currentDepthLevel + 1, targetDepth);
                dto.Replies.Add(replyDto); // Add individual DTO a la lista
            }
        }
        
        return dto;

        // if (currentDepthLevel >= targetDepth || replyCountForThisComment <= 0 || replyCountForThisComment > MaxRepliesToLoadThreshold)
        //     return dto;
        //
        // var repliesData = await _unitOfWork.Comments.GetRepliesWithReplyCountAsync(comment.Id);
        //
        // foreach (var replyData in repliesData)
        // {
        //     var replyDto = await BuildCommentWithRepliesAsync(replyData.Comment, replyData.ReplyCount,
        //         currentDepthLevel + 1, targetDepth);
        //     dto.Replies.Add(replyDto);
        // }
    }
}