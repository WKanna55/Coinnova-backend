using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;

namespace Coinnova.Infrastructure.Repositories;

public class EventCategoryRepository : Repository<EventCategory>, IEventCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public EventCategoryRepository(ApplicationDbContext context) : base(context)
    {
        this._context = context;
    }
}