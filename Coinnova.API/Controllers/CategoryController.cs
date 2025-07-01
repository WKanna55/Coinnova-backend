using Coinnova.Application.UseCases.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Obtiene la lista de todas las categorías disponibles.
    /// </summary>
    /// <returns>Una lista de categorías.</returns>
    /// <response code="200">Lista de categorías obtenida exitosamente.</response>
    /// <response code="401">Usuario no autorizado. Se requiere autenticación.</response>
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await mediator.Send(new GetCategoriesQuery());
        return Ok(categories);
    }
}