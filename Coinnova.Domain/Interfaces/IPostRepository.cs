using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface IPostRepository : IRepository<Post>
{
    Task<IOrderedQueryable<Post>> GetCommunitiesPostsForUserId(int id);
    Task<(IEnumerable<Post> Posts, int TotalCount)> GetPostsByUserIdAsync(int userId, int skip = 0, int take = 10);
    Task<Post?> GetPostDetailsByIdAsync(int postId);
}