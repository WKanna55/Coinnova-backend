using Coinnova.Application.Dtos.Event;
using Coinnova.Domain.Entities;

namespace Coinnova.Application.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventPreviewDto>> GetEventsForCommunityAsync(int communityId, int skip, int? take = null);
    Task<EventDetailDto> GetEventDetailAsync(int eventId);
}