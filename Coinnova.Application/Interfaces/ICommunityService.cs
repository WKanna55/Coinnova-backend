using Coinnova.Application.Dtos.Category;
using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Community;

namespace Coinnova.Application.Interfaces;

public interface ICommunityService
{
    Task<List<CommunityGetDto>> Get5PopularCommunities();
    Task<PagedResponseDto<CommunityWithMembersDto>> GetCommunitiesByCategoryIdAndCriteria(int id, string criteria,
        int skip, int take);

    Task<PagedResponseDto<CommunityWithMembersDto>> GetAllCommunitiesWithMembers(string criteria, int skip, int take);
}