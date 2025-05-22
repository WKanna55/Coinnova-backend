using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize(Roles = "standard")]
[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly ICommunityService _communityService;

    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

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