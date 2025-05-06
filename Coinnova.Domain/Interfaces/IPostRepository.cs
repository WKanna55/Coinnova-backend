using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface IPostRepository : IRepository<Post>
{
    Task<IOrderedQueryable<Post>> GetCommunitiesPostsForUserId(int id);
}