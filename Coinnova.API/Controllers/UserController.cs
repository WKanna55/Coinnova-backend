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
    
    /// <summary>
    /// Obtiene los datos de un usuario por su ID.
    /// </summary>
    /// <param name="id">ID del usuario a consultar.</param>
    /// <returns>Datos del usuario correspondiente.</returns>
    /// <response code="200">Usuario encontrado exitosamente.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene el rol requerido.</response>
    /// <response code="404">No se encontró un usuario con el ID proporcionado.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var user = await _userService.GetUserById(id);

        if (user == null) 
            return NotFound(new { message = $"Usuario con ID: {id} no encotrado." });
        
        return Ok(user);
    }
    
    /// <summary>
    /// Esta mal? no usar puesto que se piensa que se va a borrar
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUserInfo(int userId)
    {
        var user = await _userService.GetUserInfoById(userId);
        return Ok(user);
    }
    
    /// <summary>
    /// Edita el perfil del usuario.
    /// </summary>
    /// <param name="dto">Datos actualizados del usuario.</param>
    /// <returns>Información actualizada del usuario.</returns>
    /// <response code="200">Perfil actualizado exitosamente.</response>
    /// <response code="400">Datos inválidos en la solicitud.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene el rol requerido.</response>
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

    /// <summary>
    /// Obtiene los primeros miembros de una comunidad por su ID.
    /// </summary>
    /// <param name="id">ID de la comunidad.</param>
    /// <returns>Lista de miembros de la comunidad.</returns>
    /// <response code="200">Miembros obtenidos exitosamente.</response>
    [HttpGet("/community/{id}/members")]
    public async Task<IActionResult> GetFirstMembers([FromRoute] int id)
    {
        var members = await _userService.GetFirstCommunityMembers(id);
        return Ok(members);
    }
}