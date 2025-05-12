using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IQueryable<object>> GetQueryCommunitiesByCategoryId(int id);
}