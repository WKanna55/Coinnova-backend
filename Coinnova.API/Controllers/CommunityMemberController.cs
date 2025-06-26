using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize(Roles = "standard")]
[Route("api/[controller]")]
public class CommunityMemberController : ControllerBase
{
    public readonly ICommunityMemberService _communityMemberService;

    public CommunityMemberController(ICommunityMemberService communityMemberService)
    {
        _communityMemberService = communityMemberService;
    }

    [HttpPost("{userId}/{communityId}")]
    public async Task<IActionResult> SubscribedUser(int userId, int communityId)
    {
        var subscribed = await _communityMemberService.SubscribeUserToCommunity(userId, communityId);
        if (subscribed) return Ok(subscribed);
        return BadRequest(subscribed);
    }
}