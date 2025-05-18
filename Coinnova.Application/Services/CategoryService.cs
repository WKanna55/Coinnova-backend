using Coinnova.Application.Dtos.Category;
using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using Mapster;

namespace Coinnova.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetCategories()
    {
        var categories = await _unitOfWork.Categories.GetAll();
        var response = categories.Adapt<IEnumerable<CategoryResponseDto>>();
        return response;
    }
}