using Coinnova.Application.Dtos.Category;

namespace Coinnova.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetCategories();
}