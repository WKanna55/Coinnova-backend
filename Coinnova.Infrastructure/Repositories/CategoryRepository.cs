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

    public Task<IQueryable<object>> GetQueryCommunitiesByCategoryId(int id)
    {
        var query =  _context.CommunityCategory
            .Where(cc => cc.IdCategory == id)
            .Select(cc => cc.IdCommunity)
            .Distinct()
            .Join(
                _context.Community,
                communityId => communityId,
                c => c.Id,
                (communityId, c) => new { Community = c }
            )
            .GroupJoin(
                _context.CommunityMember,
                c => c.Community.Id,
                cm => cm.IdCommunity,
                (c, members) => new 
                {
                    Id = c.Community.Id,
                    Name = c.Community.Name,
                    Description = c.Community.Description,
                    ImageUrl = c.Community.Imageurl,
                    CreatedAt = c.Community.Createdat,
                    Members = members.Count()
                });

        return Task.FromResult<IQueryable<object>>(query);
    }
    
}