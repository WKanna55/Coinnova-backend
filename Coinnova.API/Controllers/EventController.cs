using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("community/{communityId}/events")]
    public async Task<IActionResult> GetEventsForCommunity(int communityId, [FromQuery]int skip, [FromQuery] int? take = null)
    {
        var events = await _eventService.GetEventsForCommunityAsync(communityId, skip, take);
        return Ok(events);
    }

    [HttpGet("eventDetail/{eventId}")]
    public async Task<IActionResult> GetEventDetail(int eventId)
    {
        var eventDetail = await _eventService.GetEventDetailAsync(eventId);
        return Ok(eventDetail);
    }
}