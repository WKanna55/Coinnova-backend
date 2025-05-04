using Coinnova.Application.Dtos.Category;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

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
        var categories = await _unitOfWork.Repository<Category>().GetAll();
        return categories.Select(c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name
        });
    }
}