using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface ICommunityRepository: IRepository<Community>
{
    Task<int> CountCommunityMembersByCommunityId(int id);
    IQueryable<Community> QueryComunityWithMembers();
    IQueryable<Community> GetQueryCommunitiesByCategoryId(int categoryId);
    IQueryable<Community> GetQueryCommunities();
}