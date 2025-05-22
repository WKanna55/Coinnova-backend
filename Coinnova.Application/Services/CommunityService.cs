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
            MemberCount = c.CommunityMember.Count()
        }).Take(5).ToListAsync();

        return communities;

    }
    
    public async Task<IEnumerable<CommunityDto>> Get12CommunitiesByCriteria(string criteria, int? categoryId = null)
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

        return communities.Adapt<IEnumerable<CommunityDto>>();
    }
}