using System.Security.Claims;
using Coinnova.Application.Dtos.User;
using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize(Roles = "standard")]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var user = await _userService.GetUserById(id);

        if (user == null) 
            return NotFound(new { message = $"Usuario con ID: {id} no encotrado." });
        
        return Ok(user);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserInfo(int userId)
    {
        var user = await _userService.GetUserInfoById(userId);
        return Ok(user);
    }
    
    [HttpPut]
    public async Task<IActionResult> EditProfile([FromBody] UpdateUserRequestDto dto)
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                    ?? User.FindFirstValue("UserId");
    
        if (claim == null || !int.TryParse(claim, out var userId))
            return Unauthorized();
    
        var response = await _userService.UpdateUserAsync(userId, dto);
        return Ok(response);
    }

    [HttpGet("/community/{id}/members")]
    public async Task<IActionResult> GetFirstMembers([FromRoute] int id)
    {
        var members = await _userService.GetFirstCommunityMembers(id);
        return Ok(members);
    }
}