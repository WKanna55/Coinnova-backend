using Coinnova.Application.Dtos.Category;
using Coinnova.Application.Dtos.Common;

namespace Coinnova.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetCategories();

}