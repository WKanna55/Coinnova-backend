using Coinnova.Application.Dtos.Comment;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;

namespace Coinnova.Application.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CommentWithRepliesDto>> GetCommentsWithRepliesByPostIdAsync(int postId)
    {
        var rootComments = await _unitOfWork.Comments.GetAllRootCommentsByPostId(postId);
        var result = new List<CommentWithRepliesDto>();

        foreach (var root in rootComments)
        {
            var rootDto = await BuildCommentWithRepliesAsync(root, 3);
            result.Add(rootDto);
        }

        return result;
    }
    
    private async Task<CommentWithRepliesDto> BuildCommentWithRepliesAsync(Comment comment, int depth)
    {
        // Creamos el DTO base
        var dto = comment.Adapt<CommentWithRepliesDto>();

        if (depth <= 0) 
        {
            // Si ya no queremos profundizar, RepliesCount = 0
            dto.RepliesCount = 0;
            return dto;
        }

        // 1) Obtenemos todas las respuestas directas
        var replies = await _unitOfWork.Comments.GetAllRepliesByCommentId(comment.Id);

        // 2) Fijamos el contador en función del número de replies directas
        dto.RepliesCount = replies.Count();

        // 3) Para cada respuesta, creamos su DTO recursivamente
        foreach (var reply in replies.OrderBy(r => r.Createdat))
        {
            var replyDto = await BuildCommentWithRepliesAsync(reply, depth - 1);
            dto.Replies.Add(replyDto);
        }

        return dto;
    }
}