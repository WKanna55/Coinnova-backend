using Coinnova.Application.Dtos.Event;
using Coinnova.Domain.Entities;

namespace Coinnova.Application.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventPreviewDto>> GetTop6EventsForCommunity(int communityId);
    Task<EventDto> CreateEvent(CreateEventDto eventDto);
    Task<bool> UploadEventImage(UploadEventImageDto uploadEventImageDto);
    Task<bool> UploadEventDocument(UploadEventDocumentDto uploadEventDocumentDto);
}