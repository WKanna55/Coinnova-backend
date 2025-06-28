using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface ICommunityRepository: IRepository<Community>
{
    Task<int> CountCommunityMembersByCommunityId(int id);
    IQueryable<Community> QueryComunityWithMembers();
    IQueryable<Community> GetQueryCommunitiesByCategoryId(int categoryId);
    IQueryable<Community> GetQueryCommunities();
    Task<List<Community>> GetForInstitutions(int institutionId);
    Task<IList<int>> GetIdsForSuscribedUserGeneral(int userId);
    Task<IList<int>> GetIdsForSuscribedUserInstitution(int userId);
    Task<IEnumerable<Community>> SearchCommunitiesByName(string name);
}