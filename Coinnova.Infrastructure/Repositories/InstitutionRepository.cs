using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Repositories;

public class InstitutionRepository : Repository<Institution>, IInstitutionRepository
{
    private readonly ApplicationDbContext _context;

    public InstitutionRepository(ApplicationDbContext context) : base(context)
    {
        this._context = context;
    }

    public async Task<Institution?> GetByDomainAsync(string domain)
    {
        return await _context.Institution.FirstOrDefaultAsync(i => i.Domain != null && i.Domain.ToLower() == domain.ToLower());
    }
}