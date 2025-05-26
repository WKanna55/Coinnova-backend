using Coinnova.Application.Dtos.Event;
using Coinnova.Application.Dtos.User;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using MapsterMapper;

namespace Coinnova.Application.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;


    public EventService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EventPreviewDto>> GetEventsForCommunityAsync(int communityId, int skip, int? take = null)
    {
        var result = await _unitOfWork.Events.GetEventsForCommunitySources(communityId, skip, take);

        if (skip > 0) result = result.Skip(skip);
        if (take.HasValue) result = result.Take(take.Value);
        
        return result.Cast<EventPreviewDto>();
    }

    public async Task<EventDetailDto> GetEventDetailAsync(int eventId)
    {
        var ev = await _unitOfWork.Events.GetEventDetailByIdAsync(eventId);
        if (ev == null) return null;

        return new EventDetailDto
        {
            Id = ev.Id,
            Name = ev.Name,
            Place = ev.Place,
            Description = ev.Description,
            InitialDate = ev.Initialdate,
            EndDate = ev.Enddate,
            RulesUrl = ev.Rulesurl
        };
    }
}