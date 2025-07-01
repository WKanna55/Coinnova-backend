using Coinnova.Application.Dtos.Comment;
using Coinnova.Application.Interfaces;
using Coinnova.Application.UseCases.Comments.Commands;
using Coinnova.Application.UseCases.Comments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize]
[Route("api/comment")]
public class CommentController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Obtiene todos los comentarios asociados a una publicación, con sus respuestas anidadas según la profundidad indicada.
    /// </summary>
    /// <param name="postId">ID de la publicación.</param>
    /// <param name="depth">
    /// Nivel de profundidad para incluir respuestas anidadas. 
    /// Si no se especifica, se devuelve 3 de depth.
    /// </param>
    /// <returns>Lista de comentarios con sus respuestas anidadas (si aplica).</returns>
    /// <response code="200">Comentarios obtenidos exitosamente.</response>
    /// <response code="401">Usuario no autorizado.</response>
    /// <response code="403">El usuario no tiene el rol requerido para acceder a este recurso.</response>
    
    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetAllCommentsByPostId([FromRoute] int postId, [FromQuery] int? depth)
    {
        return Ok(await mediator.Send(new GetCommentsWithRepliesByPostIdQuery
        {
            PostId = postId,
            RequestDepth = depth
        }));
    }

    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetCommentWithRepliesByParentCommentId([FromRoute] int commentId, [FromQuery] int? depth)
    {
        return Ok(await mediator.Send(new GetCommentRepliesQuery
        {
            CommentId = commentId,
            RequestDepth = depth
        }));
    }

    [HttpPost("createComment")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand command)
    {
        var created = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateComment), new { id = created.Id }, created);
    }
}