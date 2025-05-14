using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    Task<IEnumerable<object>> GetTop6EventsForCommunity(int communityId);
}