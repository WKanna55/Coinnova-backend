using Coinnova.Application.Dtos.Community;

namespace Coinnova.Application.Interfaces;

public interface ICommunityService
{
    Task<List<CommunityGetDto>> Get5PopularCommunities();
}