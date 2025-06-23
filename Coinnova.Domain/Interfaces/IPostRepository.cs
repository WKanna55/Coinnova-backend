using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface IPostRepository : IRepository<Post>
{
    Task<(IEnumerable<Post> Posts, int totalCount)> GetPostsByUserIdAsync(int userId, int skip, int take);
    Task<IOrderedQueryable<Post>> QueryPostsForUser(int userId);
    Task<Post?> GetPostDetailsByIdAsync(int postId);
    Task<(IEnumerable<Post> Posts, int totalCount)> GetPostsByCommunityId(int communityId, int skip, int take);
    Task<Post?> LikePostById(int postId);
    Task<IEnumerable<Post>> GetForCommunityIds(IList<int> communityIds, int skip, int take);
    Task<int> CountPostsAsync(IList<int> communityIds);
}