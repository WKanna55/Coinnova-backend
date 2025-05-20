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

    [HttpGet("populars")]
    public async Task<IActionResult> GetPopular()
    {
        var communities = await _communityService.Get5PopularCommunities();

        return Ok(communities);
    }
    
    [HttpGet("category/{id}")]
    public async Task<IActionResult> GetCommunitiesByCategoryIdAndCriteria([FromRoute] int id, [FromQuery] string criteria, [FromQuery] int skip, [FromQuery] int take)
    {
        var response = await _communityService.GetCommunitiesByCategoryIdAndCriteria(id, criteria, skip, take);
        return Ok(response);
    }

    [HttpGet("all-with-members")]
    public async Task<IActionResult> GetAllCommunitiesWithMembers([FromQuery] string criteria, [FromQuery] int skip,
        [FromQuery] int take)
    {
        var response = await _communityService.GetAllCommunitiesWithMembers(criteria,skip, take);
        return Ok(response);
    }
    
}