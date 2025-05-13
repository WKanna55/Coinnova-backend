using Coinnova.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coinnova.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetCategories();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommunitiesByCategoryIdAndCriteria([FromRoute] int id, [FromQuery] string criteria, [FromQuery] int skip, [FromQuery] int take)
    {
        var response = await _categoryService.GetCommunitiesByCategoryIdAndCriteria(id, criteria, skip, take);
        return Ok(response);
    }
}