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

    /// <summary>
    /// Crea un nuevo evento y lo une a instituciones y/o categorias.
    /// </summary>
    /// <param name="createEventDto">Datos del evento a crear.</param>
    /// <returns>El evento creado.</returns>
    /// <response code="200">Evento creado exitosamente.</response>
    /// <response code="400">Datos del formulario inválidos.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene permisos para acceder a este recurso.</response>
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto createEventDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var eventT = await _eventService.CreateEvent(createEventDto);
        return Ok(eventT);

    }
}