using Coinnova.Application.Dtos.Comment;
using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetAllCommentsByPostId(int postId, [FromQuery] int? depth)
    {
        var comments = await _commentService.GetCommentsWithRepliesByPostIdAsync(postId, depth);
        return Ok(comments);
    }
}