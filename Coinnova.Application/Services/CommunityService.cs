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
        var query = _unitOfWork.Communities.GetPopularCommunities();

        var communities = await query.Take(5).ToListAsync();

        return communities.Adapt<List<CommunityGetDto>>();

    }
    
}