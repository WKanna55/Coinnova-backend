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
    
    /// <summary>
    /// Obtiene la lista de todas las categorías disponibles.
    /// </summary>
    /// <returns>Una lista de categorías.</returns>
    /// <response code="200">Lista de categorías obtenida exitosamente.</response>
    /// <response code="401">Usuario no autorizado. Se requiere autenticación.</response>
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetCategories();
        return Ok(categories);
    }
}