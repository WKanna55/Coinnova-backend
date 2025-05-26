using Coinnova.Application.Dtos.Institution;

namespace Coinnova.Application.Interfaces;

public interface IInstitutionService
{
    Task<List<InstitutionSummaryDto>> GetAllInstitutionsSummary();
}