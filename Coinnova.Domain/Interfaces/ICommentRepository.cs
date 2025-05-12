using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetAllRootCommentsByPostId(int postId);
    Task<IEnumerable<Comment>> GetAllRepliesByCommentId(int commentId);
}