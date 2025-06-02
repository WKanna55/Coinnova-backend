using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]

[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly ICommunityService _communityService;

    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    /// <summary>
    /// Obtiene las 5 comunidades más populares (más n. de usuarios).
    /// </summary>
    /// <returns>Una lista de las comunidades con mayor popularidad(n. usuarios).</returns>
    /// <response code="200">Comunidades populares obtenidas exitosamente.</response>
    [HttpGet("populars")]
    public async Task<IActionResult> GetPopular()
    {
        var communities = await _communityService.Get5PopularCommunities();

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
        var response = await _communityService.Get12CommunitiesByCriteria(criteria, categoryId);
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
        var response = await _communityService.Get12CommunitiesByCriteria(criteria);
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
        var communities = await _communityService.GetByInstitutionId(institutionId);
        return Ok(communities);
    }
    
}