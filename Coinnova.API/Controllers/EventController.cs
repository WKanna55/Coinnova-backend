using Coinnova.Application.Dtos.Event;
using Coinnova.Application.Interfaces;
using Coinnova.Application.UseCases.Events.Commands;
using Coinnova.Application.UseCases.Events.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EventController(IMediator mediator) : ControllerBase
{
    [HttpGet("community/{communityId}/events")]
    public async Task<IActionResult> GetEventsForCommunity(int communityId, [FromQuery]int skip, [FromQuery] int? take = null)
    {
        var query = new GetEventsForCommunityQuery(communityId, skip, take);
        var events = await mediator.Send(query);
        return Ok(events);
    }

    [HttpGet("{eventId}")]
    public async Task<IActionResult> GetEventDetail(int eventId)
    {
        var query = new GetEventDetailQuery(eventId);
        var eventDetail = await mediator.Send(query);
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
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto createEventDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var command = new CreateEventCommand(createEventDto);
        var eventT = await mediator.Send(command);
        return Ok(eventT);
    }
}