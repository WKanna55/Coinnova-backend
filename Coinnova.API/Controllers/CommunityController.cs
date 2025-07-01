using Coinnova.Application.UseCases.Communities.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommunityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene las 5 comunidades más populares (más n. de usuarios).
    /// </summary>
    /// <returns>Una lista de las comunidades con mayor popularidad(n. usuarios).</returns>
    /// <response code="200">Comunidades populares obtenidas exitosamente.</response>
    [HttpGet("populars")]
    public async Task<IActionResult> GetPopular()
    {
        var query = new GetPopularCommunitiesQuery();
        var communities = await _mediator.Send(query);

        return Ok(communities);
    }
    
    /// <summary>
    /// Obtiene hasta 12 comunidades de una categoría específica según el criterio de ordenamiento.
    /// </summary>
    /// <param name="categoryId">ID de la categoría a filtrar.</param>
    /// <param name="criteria">
    /// Criterio de ordenamiento de las comunidades:
    /// <list type="bullet">
    /// <item><term>popular</term><description> Ordena por cantidad de miembros (descendente).</description></item>
    /// <item><term>new</term><description> Ordena por fecha de creación (más recientes primero).</description></item>
    /// </list>
    /// </param>
    /// <returns>Una lista de hasta 12 comunidades filtradas por categoría y ordenadas por el criterio especificado.</returns>
    /// <response code="200">Comunidades obtenidas exitosamente.</response>
    /// <response code="400">Parámetros incorrectos.</response>
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetCommunitiesByCategoryIdAndCriteria(
        [FromRoute] int categoryId, 
        [FromQuery] string criteria)
    {
        var query = new GetCommunitiesByCriteriaQuery(criteria, categoryId);
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    /// <summary>
    /// Obtiene hasta 12 comunidades de cualquier categoria, ordenadas según el criterio especificado.
    /// </summary>
    /// <param name="criteria">
    /// Criterio de ordenamiento de las comunidades:
    /// <list type="bullet">
    /// <item><term>popular</term><description> Ordena por cantidad de miembros (descendente).</description></item>
    /// <item><term>new</term><description> Ordena por fecha de creación (más recientes primero).</description></item>
    /// </list>
    /// </param>
    /// <returns>Una lista de hasta 12 comunidades de cualquier categoria ordenadas por el criterio especificado.</returns>
    /// <response code="200">Comunidades obtenidas exitosamente.</response>
    [HttpGet("category")]
    public async Task<IActionResult> GetCommunitiesByCriteria([FromQuery] string criteria)
    {
        var query = new GetCommunitiesByCriteriaQuery(criteria);
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    /// <summary>
    /// Obtiene todas las comunidades de una institucion
    /// </summary>
    /// <param name="institutionID">
    /// Id de la institucion
    /// </param>
    /// <returns>Una lista de todas las categorias de una institucion</returns>
    /// <response code="200">Comunidades obtenidas exitosamente.</response>
    [HttpGet("institution/{institutionId}")]
    public async Task<IActionResult> GetByInstitutionId([FromRoute] int institutionId)
    {
        var query = new GetCommunitiesByInstitutionIdQuery(institutionId);
        var communities = await _mediator.Send(query);
        return Ok(communities);
    }

    /// <summary>
    /// Busca comunidades por nombre con paginación.
    /// </summary>
    /// <param name="name">Nombre o parte del nombre a buscar.</param>
    /// <param name="skip">Cantidad de comunidades a omitir (paginación).</param>
    /// <param name="take">Cantidad de comunidades a retornar (paginación).</param>
    /// <returns>Una respuesta paginada con las comunidades que coinciden con el nombre.</returns>
    /// <response code="200">Comunidades encontradas exitosamente.</response>
    /// <response code="400">Parámetros inválidos para paginación.</response>
    [HttpGet("search")]
    public async Task<IActionResult> SearchCommunitiesByName([FromQuery] string name, [FromQuery] int skip, [FromQuery] int take)
    {
        var query = new SearchCommunitiesByNameQuery(name, skip, take);
        var communities = await _mediator.Send(query);
        return Ok(communities);
    }
}