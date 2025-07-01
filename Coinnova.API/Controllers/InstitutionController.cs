using Coinnova.Application.UseCases.Institutions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize(Roles="admin")]
[Route("api/[controller]")]
public class InstitutionController : ControllerBase
{
    private readonly IMediator _mediator;

    public InstitutionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene un resumen de todas las instituciones registradas. Requiere rol de administrador.
    /// </summary>
    /// <returns>Una lista de resúmenes de instituciones.</returns>
    /// <response code="200">Resumen de instituciones obtenido exitosamente.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene permisos de administrador.</response>
    [HttpGet("all-summary")]
    public async Task<IActionResult> GetAllInstitutionsSummary()
    {
        var query = new GetAllInstitutionsSummaryQuery();
        var institutions = await _mediator.Send(query);
        
        return Ok(institutions);
    }
}