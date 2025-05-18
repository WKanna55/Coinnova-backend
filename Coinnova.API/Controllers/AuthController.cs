using Coinnova.Application.Dtos.Auth;
using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Coinnova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var user = await _authService.Login(loginDto);

        if (user == null) 
            return NotFound();

        return Ok(user);
    }

    [HttpPost("register")]
    [EnableRateLimiting("FixedLimiting")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var userDto = await _authService.Register(registerDto);
        
        return Ok(userDto);
    }

    [Authorize(Roles = "standard")]
    [HttpGet("pruebaAuth")]
    public IActionResult Prueba()
    {
        return Ok();
    }
    


}