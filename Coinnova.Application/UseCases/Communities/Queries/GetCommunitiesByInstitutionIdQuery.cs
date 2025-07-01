using Coinnova.Application.Dtos.Community;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Communities.Queries;

public record GetCommunitiesByInstitutionIdQuery(int InstitutionId) : IRequest<List<CommunityWithNMembersDto>>;

public class GetCommunitiesByInstitutionIdQueryHandler : IRequestHandler<GetCommunitiesByInstitutionIdQuery, List<CommunityWithNMembersDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCommunitiesByInstitutionIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CommunityWithNMembersDto>> Handle(GetCommunitiesByInstitutionIdQuery request, CancellationToken cancellationToken)
    {
        var communities = await _unitOfWork.Communities.GetForInstitutions(request.InstitutionId);
        return communities.Adapt<List<CommunityWithNMembersDto>>();
    }
} 