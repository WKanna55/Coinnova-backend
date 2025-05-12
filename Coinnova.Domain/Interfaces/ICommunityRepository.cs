using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface ICommunityRepository: IRepository<Community>
{
    Task<int> CountCommunityMembersByCommunityId(int id);
    IQueryable<object> GetPopularCommunities();
}