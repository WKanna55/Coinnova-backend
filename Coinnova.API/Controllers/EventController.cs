using Coinnova.Application.Dtos.Event;
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

    [HttpGet("community/{communityId}/top6")]
    public async Task<IActionResult> GetTop6EventsForCommunity(int communityId)
    {
        var events = await _eventService.GetTop6EventsForCommunity(communityId);
        return Ok(events);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto createEventDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var eventT = await _eventService.CreateEvent(createEventDto);
        return Ok(eventT);
    }
}