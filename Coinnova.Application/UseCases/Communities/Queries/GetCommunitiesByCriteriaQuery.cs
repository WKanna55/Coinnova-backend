using Coinnova.Application.Dtos.Community;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Application.UseCases.Communities.Queries;

public record GetCommunitiesByCriteriaQuery(string Criteria, int? CategoryId = null) : IRequest<IEnumerable<CommunityUsingBaseDto>>;

public class GetCommunitiesByCriteriaQueryHandler : IRequestHandler<GetCommunitiesByCriteriaQuery, IEnumerable<CommunityUsingBaseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private const int MaxCommunitiesToTakeByCategory = 12;

    public GetCommunitiesByCriteriaQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CommunityUsingBaseDto>> Handle(GetCommunitiesByCriteriaQuery request, CancellationToken cancellationToken)
    {
        var query = request.CategoryId.HasValue
            ? _unitOfWork.Communities.GetQueryCommunitiesByCategoryId(request.CategoryId.Value)
            : _unitOfWork.Communities.GetQueryCommunities();

        query = request.Criteria.ToLower() switch
        {
            "popular" => query.OrderByDescending(c => c.MemberCount).Take(MaxCommunitiesToTakeByCategory),
            "new" => query.OrderByDescending(c => c.Createdat).Take(MaxCommunitiesToTakeByCategory),
            _ => query.OrderByDescending(c => c.Createdat).Take(MaxCommunitiesToTakeByCategory)
        };

        var communities = await query.ToListAsync();

        return communities.Adapt<IEnumerable<CommunityUsingBaseDto>>();
    }
} 