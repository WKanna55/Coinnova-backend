using Coinnova.Application.Dtos.Institution;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Institutions.Queries;

public record GetAllInstitutionsSummaryQuery() : IRequest<List<InstitutionSummaryDto>>;

public class GetAllInstitutionsSummaryQueryHandler : IRequestHandler<GetAllInstitutionsSummaryQuery, List<InstitutionSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllInstitutionsSummaryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<InstitutionSummaryDto>> Handle(GetAllInstitutionsSummaryQuery request, CancellationToken cancellationToken)
    {
        var institutions = await _unitOfWork.InstitutionRepository.GetAll();
        
        return institutions.Adapt<List<InstitutionSummaryDto>>();
    }
} 