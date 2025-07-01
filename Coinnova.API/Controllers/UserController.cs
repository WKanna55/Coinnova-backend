using System.Security.Claims;
using Coinnova.API.Filters;
using Coinnova.Application.Dtos.User.HttpMethods;
using Coinnova.Application.UseCases.Users.Commands;
using Coinnova.Application.UseCases.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize(Roles = "standard")]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
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
        var user = await _mediator.Send(new GetUserByIdQuery { Id = id });

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
        var user = await _mediator.Send(new GetUserInfoByIdQuery { UserId = userId });
        return Ok(user);
    }

    /// <summary>
    /// Edita el perfil del usuario.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dto">Datos actualizados del usuario.</param>
    /// <returns>Información actualizada del usuario.</returns>
    /// <response code="200">Perfil actualizado exitosamente.</response>
    /// <response code="400">Datos inválidos en la solicitud.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene el rol requerido.</response>
    [HttpPatch("{userId}")]
    [AuthorizeSameUser(RouteIdName = "userId", ClaimType = JwtRegisteredClaimNames.Sub)] // etiqueta personalizada
    public async Task<IActionResult> EditProfile([FromRoute] int userId,
        [FromForm] UpdateUserRequestDto dto)
    {
        var response = await _mediator.Send(new UpdateUserCommand 
        { 
            UserId = userId, 
            UserRequestDto = dto 
        });
        return Ok(response);
    }

    /// <summary>
    /// Obtiene los primeros miembros de una comunidad por su ID.
    /// </summary>
    /// <param name="id">ID de la comunidad.</param>
    /// <returns>Lista de miembros de la comunidad.</returns>
    /// <response code="200">Miembros obtenidos exitosamente.</response>
    [HttpGet("community/{id}/members")]
    public async Task<IActionResult> GetFirstMembers([FromRoute] int id)
    {
        var members = await _mediator.Send(new GetFirstCommunityMembersQuery { CommunityId = id });
        return Ok(members);
    }
    
    
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var userIdClaim = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized("Token inválido");
        }

        var userInfo = await _mediator.Send(new GetLoggedUserInfoQuery { UserId = userId });
        if (userInfo == null)
        {
            return NotFound("Usuario no encontrado");
        }
        
        return Ok(userInfo);
    } 
    
    [HttpGet("detail/{id}")]
    [Authorize]
    public async Task<IActionResult> GetDetailedById([FromRoute] int id)
    {
        var user = await _mediator.Send(new GetDetailedByIdQuery { UserId = id });
        if (user == null)
        {
            return NotFound("Usuario no encontrado");
        }
        
        return Ok(user);
    } 
    
}