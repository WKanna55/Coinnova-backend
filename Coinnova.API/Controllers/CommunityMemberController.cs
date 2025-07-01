using Coinnova.Application.Interfaces;
using Coinnova.Application.UseCases.CommunityMembers.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CommunityMemberController(IMediator mediator) : ControllerBase
{
    [HttpPost("{userId}/{communityId}")]
    public async Task<IActionResult> SubscribedUser(int userId, int communityId)
    {
        var command = new SubscribeUserToCommunityCommand(userId, communityId);
        var response = await mediator.Send(command);
        if (response) 
            return Ok(response);
        return BadRequest(response);
    }
}