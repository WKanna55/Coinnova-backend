using Coinnova.Application.Dtos.Event;
using Coinnova.Application.Dtos.User;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MapsterMapper;

namespace Coinnova.Application.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;


    public EventService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EventPreviewDto>> GetTop6EventsForCommunity(int communityId)
    {
        var result = await _unitOfWork.Events.GetTop6EventsForCommunity(communityId);
        return result.Cast<EventPreviewDto>();
    }

    public async Task<EventDto> CreateEvent(CreateEventDto eventDto)
    {
        
        return null;
    }
    
    
}