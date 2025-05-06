using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("user-feed/{id}")]
    public async Task<IActionResult> PostsForUserId([FromRoute] int id, [FromQuery]int skip, [FromQuery] int take)
    {
        var posts = await _postService.GetPostsForUserId(id, skip, take);
        return Ok(posts);
    }

}