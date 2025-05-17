using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetRootCommentsAsync(int postId);
    Task<IEnumerable<Comment>> GetRepliesAsync(int parentCommentId);
}