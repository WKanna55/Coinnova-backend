using Coinnova.Application.Dtos.Community;
using Coinnova.Domain.Interfaces.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Application.UseCases.Communities.Queries;

public record GetPopularCommunitiesQuery() : IRequest<List<CommunityWithNMembersDto>>;

public class GetPopularCommunitiesQueryHandler : IRequestHandler<GetPopularCommunitiesQuery, List<CommunityWithNMembersDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPopularCommunitiesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CommunityWithNMembersDto>> Handle(GetPopularCommunitiesQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Communities.QueryComunityWithMembers();

        var communities = await query
            .OrderByDescending(c => c.CommunityMember.Count())
            .Select(c => new CommunityWithNMembersDto
            {
                Id = c.Id,
                Name = c.Name,
                MemberCount = c.CommunityMember.Count(),
                Imageurl = c.Imageurl
            })
            .Take(5)
            .ToListAsync();

        return communities;
    }
} 