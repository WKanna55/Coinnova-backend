using Coinnova.Application.Dtos.Category;
using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Community;

namespace Coinnova.Application.Interfaces;

public interface ICommunityService
{
    Task<List<CommunityWithNMembersDto>> Get5PopularCommunities();
    Task<IEnumerable<CommunityUsingBaseDto>> Get12CommunitiesByCriteria(string criteria, int? categoryId = null);
    Task<IEnumerable<CommunityUsingBaseDto>> SearchByName(string name);
    Task<List<CommunityWithNMembersDto>> GetByInstitutionId(int institutionId);
}