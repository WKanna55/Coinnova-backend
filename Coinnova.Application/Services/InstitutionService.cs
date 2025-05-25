using Coinnova.Application.Dtos.Institution;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using Mapster;

namespace Coinnova.Application.Services;

public class InstitutionService : IInstitutionService
{
    private readonly IUnitOfWork _unitOfWork;

    public InstitutionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<InstitutionSummaryDto>> GetAllInstitutionsSummary()
    {
        var institutions = await _unitOfWork.Institutions.GetAll();
         
        return institutions.Adapt<List<InstitutionSummaryDto>>();
    }
    
}