using Coinnova.Application.Dtos.Comment;

namespace Coinnova.Application.Interfaces;

public interface ICommentService 
{ 
    Task<IEnumerable<CommentWithRepliesDto>> GetCommentsWithRepliesByPostIdAsync(int postId);
}