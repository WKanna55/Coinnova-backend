using Coinnova.Application.Dtos.Post;
using Coinnova.Application.UseCases.Posts.Commands;
using Coinnova.Application.UseCases.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize(Roles="standard")]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene posts de las comunidades a las que esta suscrito el usuario.
    /// </summary>
    /// <param name="id">ID del usuario.</param>
    /// <param name="skip">Número de publicaciones a omitir (paginación).</param>
    /// <param name="take">Número de publicaciones a retornar (paginación).</param>
    /// <returns>Una respuesta paginada con publicaciones para el feed del usuario.</returns>
    /// <response code="200">Publicaciones obtenidas exitosamente.</response>
    /// <response code="400">Parámetros de consulta inválidos.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene el rol requerido.</response>
    [HttpGet("user-feed/{id}")]
    public async Task<IActionResult> GeneralPostsForUserId([FromRoute] int id, [FromQuery]int skip, [FromQuery] int take)
    {
        var query = new GetUserFeedQuery(id, skip, take);
        var posts = await _mediator.Send(query);
        return Ok(posts);
    }

    /// <summary>
    /// Obtiene los detalles de una post específico mediante su ID.
    /// </summary>
    /// <param name="postId">ID de la publicación.</param>
    /// <returns>Los detalles de la publicación solicitada.</returns>
    /// <response code="200">Detalles de la publicación obtenidos exitosamente.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene el rol requerido.</response>
    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPostDetails(int postId)
    {
        var query = new GetPostDetailsQuery(postId);
        var post = await _mediator.Send(query);
        return Ok(post);
    }

    /// <summary>
    /// Obtiene los posts creados por un usuario específico.
    /// </summary>
    /// <param name="userId">ID del usuario del que se desean obtener las publicaciones.</param>
    /// <param name="skip">Cantidad de publicaciones a omitir (paginación).</param>
    /// <param name="take">Cantidad de publicaciones a retornar (paginación).</param>
    /// <returns>Una respuesta paginada con las publicaciones del usuario.</returns>
    /// <response code="200">Publicaciones obtenidas exitosamente.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene el rol requerido.</response>
    /// <response code="400">Parámetros inválidos para paginación.</response>
    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetPostsByUserId(int userId, [FromQuery] int skip, [FromQuery] int take)
    {
        var query = new GetPostsByUserIdQuery(userId, skip, take);
        var posts = await _mediator.Send(query);
        return Ok(posts);
    }
    
    /// <summary>
    /// Obtiene los posts asociados a una comunidad específica.
    /// </summary>
    /// <param name="id">ID de la comunidad.</param>
    /// <param name="skip">Cantidad de publicaciones a omitir (para paginación).</param>
    /// <param name="take">Cantidad de publicaciones a retornar.</param>
    /// <returns>Una lista paginada de publicaciones de la comunidad.</returns>
    /// <response code="200">Publicaciones obtenidas exitosamente.</response>
    /// <response code="400">Parámetros de paginación inválidos.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene el rol requerido.</response>
    /// <response code="404">No se encontró la comunidad especificada.</response>
    [HttpGet("community/{id}/posts")]
    public async Task<IActionResult> PostsByCommunityId([FromRoute] int id, [FromQuery] int skip, [FromQuery] int take)
    {
        var query = new GetPostsByCommunityIdQuery(id, skip, take);
        var posts = await _mediator.Send(query);
        return Ok(posts);
    }

    /// <summary>
    /// Crea una nueva publicación con o sin imagen.
    /// </summary>
    /// <param name="createPostDto">Datos del formulario para crear la publicación.</param>
    /// <returns>La publicación creada.</returns>
    /// <response code="200">Publicación creada exitosamente.</response>
    /// <response code="400">Datos inválidos en el formulario.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene el rol requerido.</response>
    [HttpPost("create")]
    public async Task<IActionResult> CreatePost([FromForm] CreatePostDto createPostDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
            
        var command = new CreatePostCommand(createPostDto);
        var post = await _mediator.Send(command);
        return Ok(post);
    }

    [HttpPost("post/{id}/like")]
    public async Task<IActionResult> LikeAPost([FromRoute] int id)
    {
        var command = new LikePostCommand(id);
        var post = await _mediator.Send(command);
        
        if (post == null) 
            return NotFound();

        return Ok(new { post.Id, post.Likes });
    }
    
    [HttpGet("institution-user-feed/{id}")]
    public async Task<IActionResult> InstitutionPostsForUserId([FromRoute] int id, [FromQuery]int skip, [FromQuery] int take)
    {
        var query = new GetInstitutionUserFeedQuery(id, skip, take);
        var posts = await _mediator.Send(query);
        return Ok(posts);
    }

    /// <summary>
    /// Busca publicaciones por título con paginación.
    /// </summary>
    /// <param name="title">Título o parte del título a buscar.</param>
    /// <param name="skip">Cantidad de publicaciones a omitir (paginación).</param>
    /// <param name="take">Cantidad de publicaciones a retornar (paginación).</param>
    /// <returns>Una respuesta paginada con las publicaciones que coinciden con el título.</returns>
    /// <response code="200">Publicaciones encontradas exitosamente.</response>
    /// <response code="400">Parámetros inválidos para paginación.</response>
    /// <response code="401">No autorizado. El usuario no ha iniciado sesión.</response>
    /// <response code="403">Prohibido. El usuario no tiene el rol requerido.</response>
    [HttpGet("search")]
    public async Task<IActionResult> SearchPostsByTitle([FromQuery] string title, [FromQuery] int skip, [FromQuery] int take)
    {
        var query = new SearchPostsByTitleQuery(title, skip, take);
        var posts = await _mediator.Send(query);
        return Ok(posts);
    }
}
