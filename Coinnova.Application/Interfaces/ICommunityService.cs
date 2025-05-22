using Coinnova.Application.Dtos.Category;
using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Community;

namespace Coinnova.Application.Interfaces;

public interface ICommunityService
{
    Task<List<CommunityGetDto>> Get5PopularCommunities();
    Task<IEnumerable<CommunityDto>> Get12CommunitiesByCriteria(string criteria, int? categoryId = null);
}