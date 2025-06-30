using Coinnova.Application.Dtos.Comment;
using Coinnova.Domain.Entities;

namespace Coinnova.Application.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentWithRepliesDto>> GetCommentsWithRepliesByPostIdAsync(int postId, int? requestDepth = null);
    Task<IEnumerable<CommentWithRepliesDto>> GetCommentReplies(int commentId, int? requestDepth = null);
    Task<Comment> CreateComment(CreateCommentDto createCommentDto);
}