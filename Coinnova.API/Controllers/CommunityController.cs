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
    
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetCommunitiesByCategoryIdAndCriteria(
        [FromRoute] int categoryId, 
        [FromQuery] string criteria)
    {
        var response = await _communityService.Get12CommunitiesByCriteria(criteria, categoryId);
        return Ok(response);
    }

    [HttpGet("category")]
    public async Task<IActionResult> GetCommunitiesByCriteria([FromQuery] string criteria)
    {
        var response = await _communityService.Get12CommunitiesByCriteria(criteria);
        return Ok(response);
    }
}