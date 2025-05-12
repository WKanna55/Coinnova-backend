using Coinnova.Application.Dtos.Category;
using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using Mapster;

namespace Coinnova.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetCategories()
    {
        var categories = await _unitOfWork.Categories.GetAll();
        var response = categories.Adapt<IEnumerable<CategoryResponseDto>>();
        return response;
    }

    public async Task<PagedResponseDto<CommunityWithMembersDto>> GetCommunitiesByCategoryIdAndCriteria(int id, string criteria, int skip, int take)
    {
        var query = await _unitOfWork.Categories.GetQueryCommunitiesByCategoryId(id);
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

}