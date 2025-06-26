using Coinnova.Application.Dtos.Comment;
using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize(Roles = "standard")]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

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
        var comments = await _commentService.GetCommentsWithRepliesByPostIdAsync(postId, depth);
        return Ok(comments);
    }

    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetCommentWithRepliesByParentCommentId([FromRoute] int commentId, int? depth)
    {
        var commentsWithReplies = await _commentService.GetCommentReplies(commentId, depth);
        return Ok(commentsWithReplies);
    }

    [HttpPost("createComment")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createCommentDto)
    {
        var createdComment = await _commentService.CreateComment(createCommentDto);
        return CreatedAtAction(nameof(CreateComment), new { id = createdComment.Id }, createdComment);
    }
}