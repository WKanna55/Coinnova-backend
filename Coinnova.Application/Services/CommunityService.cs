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
    private const int MaxCommunitiesToTakeByCategory = 12;

    public CommunityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<CommunityWithNMembersDto>> Get5PopularCommunities()
    {
        var query = _unitOfWork.Communities.QueryComunityWithMembers();

        var communities = await query
            .OrderByDescending(c => c.CommunityMember.Count())
            .Select(c => new CommunityWithNMembersDto
            {
                Id = c.Id,
                Name = c.Name,
                MemberCount = c.CommunityMember.Count(),
                ImageUrl = c.Imageurl
            })
            .Take(5)
            .ToListAsync();

        return communities;
    }
    
    public async Task<IEnumerable<CommunityUsingBaseDto>> Get12CommunitiesByCriteria(string criteria, int? categoryId = null)
    {
        var query = categoryId.HasValue
            ? _unitOfWork.Communities.GetQueryCommunitiesByCategoryId(categoryId.Value)
            : _unitOfWork.Communities.GetQueryCommunities();

        query = criteria.ToLower() switch
        {
            "popular" => query.OrderByDescending(c => c.MemberCount).Take(MaxCommunitiesToTakeByCategory),
            "new" => query.OrderByDescending(c => c.Createdat).Take(MaxCommunitiesToTakeByCategory),
            _ => query.OrderByDescending(c => c.Createdat).Take(MaxCommunitiesToTakeByCategory)
        };

        var communities = await query.ToListAsync();

        return communities.Adapt<IEnumerable<CommunityUsingBaseDto>>();
    }

    public async Task<List<CommunityDto>> GetByInstitutionId(int institutionId)
    {
        var communities = await _unitOfWork.Communities.GetForInstitutions(institutionId);
        return communities.Adapt<List<CommunityDto>>();
    }
    
}