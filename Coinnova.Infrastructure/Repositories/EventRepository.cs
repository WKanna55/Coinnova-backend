using Coinnova.Application.Dtos.Event;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Repositories;

public class EventRepository: Repository<Event>, IEventRepository
{
private readonly ApplicationDbContext _context;
    
    public EventRepository(ApplicationDbContext _context) : base(_context)
    {
        this._context = _context;
    }

    public async Task<IEnumerable<object>> GetTop6EventsForCommunity(int communityId)
    {
        var categoryIds = await _context.CommunityCategory
            .Where(cc => cc.IdCommunity == communityId)
            .Select(cc => cc.IdCategory)
            .ToListAsync();
        
        var institutionId = await _context.Community
            .Where(c => c.Id == communityId)
            .Select(c => c.IdInstitution)
            .FirstOrDefaultAsync();
        
        // Eventos por categoría (con nombre de la categoría)
        var categoryEvents = from ec in _context.EventCategory
            join ev in _context.Event on ec.IdEvent equals ev.Id
            join cat in _context.Category on ec.IdCategory equals cat.Id
            where categoryIds.Contains(ec.IdCategory)
            select new EventPreviewDto
            {
                Id = ev.Id,
                Name = ev.Name,
                ImageUrl = ev.Imageurl,
                InitialDate = ev.Initialdate,
                SourceName = cat.Name
            };
        
        // Eventos por institución (con nombre de la institución)
        var institutionEvents = from ie in _context.InstitutionEvent
            join ev in _context.Event on ie.IdEvent equals ev.Id
            join inst in _context.Institution on ie.IdInstitution equals inst.Id
            where ie.IdInstitution == institutionId
            select new EventPreviewDto
            {
                Id = ev.Id,
                Name = ev.Name,
                ImageUrl = ev.Imageurl,
                InitialDate = ev.Initialdate,
                SourceName = inst.Name
            };
        
        // Unión, eliminación de duplicados por ID y top 6 ordenado por fecha
        var result = await categoryEvents
            .Union(institutionEvents)
            .OrderBy(e => e.InitialDate)
            .Take(6)
            .ToListAsync();

        return result;
    }
}