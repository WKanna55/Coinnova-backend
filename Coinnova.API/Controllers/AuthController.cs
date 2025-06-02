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

    /// <summary>
    /// Inicia sesión de un usuario y devuelve un token JWT junto con su correo electrónico.
    /// </summary>
    /// <param name="loginDto">Datos de inicio de sesión: email y contraseña.</param>
    /// <returns>Token JWT y datos básicos del usuario autenticado.</returns>
    /// <response code="200">Inicio de sesión exitoso. Se devuelve el token y el email.</response>
    /// <response code="400">La solicitud es inválida (por ejemplo, datos incompletos o formato incorrecto).</response>
    /// <response code="404">Usuario no encontrado o credenciales inválidas.</response>
    [HttpPost("login")]
    [EnableRateLimiting("LoginFixedWindow")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var user = await _authService.Login(loginDto);

        if (user == null) 
            return NotFound();

        return Ok(user);
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="registerDto">Datos necesarios para el registro del usuario (nombre, email y contraseña).</param>
    /// <returns>Los datos del usuario recién registrado.</returns>
    /// <response code="200">Registro exitoso. Se devuelven los datos del nuevo usuario.</response>
    /// <response code="400">La solicitud es inválida (por ejemplo, datos faltantes o formato incorrecto).</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var userDto = await _authService.Register(registerDto);
        
        return Ok(userDto);
    }
    
    /// <summary>
    /// Inicia sesión con Google utilizando un token de ID.
    /// </summary>
    /// <param name="dto">Objeto que contiene el token de ID emitido por Google.</param>
    /// <returns>Datos del usuario autenticado y un token JWT.</returns>
    /// <response code="200">Inicio de sesión exitoso con Google.</response>
    /// <response code="400">La solicitud es inválida (por ejemplo, falta el token o está mal formado).</response>
    /// <response code="401">Token de Google inválido o no autorizado.</response>
    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        var response = await _authService.LoginWithGoogleAsync(dto.IdToken);
        if (response == null)
            return Unauthorized(new { message = "Token de Google inválido." });

        return Ok(response);
    }
    
}