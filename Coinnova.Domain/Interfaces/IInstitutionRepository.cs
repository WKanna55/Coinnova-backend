using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface IInstitutionRepository : IRepository<Institution>
{
    Task<Institution?> GetByDomainAsync(string domain);
}