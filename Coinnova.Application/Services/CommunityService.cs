using Coinnova.Application.Dtos.Category;
using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Community;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Application.Services;

public class CommunityService : ICommunityService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    /*
     * No devuelve bien puesto que no se mapea object a communitygetdto
     */
    public async Task<List<CommunityGetDto>> Get5PopularCommunities()
    {
        var query = _unitOfWork.Communities.QueryComunityWithMembers();

        var communities = await query.Select(c => new CommunityGetDto
        {
            Id = c.Id,
            Name = c.Name,
            NumberOfMembers = c.CommunityMember.Count()
        }).Take(5).ToListAsync();

        return communities;

    }
    
    public async Task<PagedResponseDto<CommunityWithMembersDto>> GetCommunitiesByCategoryIdAndCriteria(int id, string criteria, int skip, int take)
    {
        var query = await _unitOfWork.Communities.GetQueryCommunitiesByCategoryId(id);
        var communities = query.Adapt<List<CommunityWithMembersDto>>();

        if (criteria == "popular")
        {
            communities = communities.OrderByDescending(c => c.Members).ToList();
        }

        else if (criteria == "new")
        {
            communities = communities.OrderByDescending(c => c.CreatedAt).ToList();
        }
        
        var totalCommunities = communities.Count;
        
        var paginated = communities.Skip(skip).Take(take).ToList();
        
        var hasMore = totalCommunities > (skip + take);
        
        return new PagedResponseDto<CommunityWithMembersDto>
        {
            Items = paginated,
            HasMore = hasMore,
            TotalCount = totalCommunities
        };
        
    }

    public async Task<PagedResponseDto<CommunityWithMembersDto>> GetAllCommunitiesWithMembers( int skip, int take)
    {
        var query = await _unitOfWork.Communities.GetQueryCommunitiesWithMembers();
        var communities = query.Adapt<List<CommunityWithMembersDto>>();
        
        communities = communities.OrderByDescending(c => c.Members).ToList();
            
        var totalCommunities = communities.Count;
        
        var paginated = communities.Skip(skip).Take(take).ToList();
        
        var hasMore = totalCommunities > (skip + take);
        
        return new PagedResponseDto<CommunityWithMembersDto>
        {
            Items = paginated,
            HasMore = hasMore,
            TotalCount = totalCommunities
        };
    }
    
}