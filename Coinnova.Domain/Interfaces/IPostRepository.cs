using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface IPostRepository : IRepository<Post>
{
    Task<IOrderedQueryable<Post>> QueryPostsForUser(int userId);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    Task<Post?> GetPostDetailsByIdAsync(int postId);
}