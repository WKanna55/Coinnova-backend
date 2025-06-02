using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;

namespace Coinnova.Infrastructure.Repositories;

public class InstitutionEventRepository : Repository<InstitutionEvent>, IInstitutionEventRepository
{
    private readonly ApplicationDbContext _context;

    public InstitutionEventRepository(ApplicationDbContext context) : base(context)
    {
        this._context = context;
    }
}