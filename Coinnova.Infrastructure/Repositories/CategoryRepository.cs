using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Repositories;

public class CategoryRepository: Repository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        this._context = context;
    }
}