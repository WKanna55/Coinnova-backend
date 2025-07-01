using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using MediatR;

namespace Coinnova.Application.UseCases.Institutions.Queries;

public record GetInstitutionByDomainQuery(string Domain) : IRequest<Institution?>;

public class GetInstitutionByDomainQueryHandler : IRequestHandler<GetInstitutionByDomainQuery, Institution?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInstitutionByDomainQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Institution?> Handle(GetInstitutionByDomainQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.InstitutionRepository.GetByDomainAsync(request.Domain);
    }
} 