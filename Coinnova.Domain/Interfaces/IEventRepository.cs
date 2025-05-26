using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    Task<IEnumerable<object>> GetEventsForCommunitySources(int communityId, int skip, int? take = null);
    Task<Event?> GetEventDetailByIdAsync(int eventId);
    
}