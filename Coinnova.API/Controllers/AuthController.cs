using Coinnova.Application.Dtos.Auth;
using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
    {
        var response = await _authService.LoginWithGoogleAsync(dto.IdToken);
        if (response == null)
            return Unauthorized(new { message = "Token de Google inválido." });

        return Ok(response);
    }
    


}