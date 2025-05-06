using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Repositories;

public class PostRepository : Repository<Post>, IPostRepository
{
    private readonly ApplicationDbContext _context;
    
    public PostRepository(ApplicationDbContext _context) : base(_context)
    {
        this._context = _context;
    }

    public async Task<IOrderedQueryable<Post>> GetCommunitiesPostsForUserId(int id)
    {
        var communityIds = await _context.CommunityMember
            .Where(cm => cm.IdUser == id)
            .Select(cm => cm.IdCommunity)
            .ToListAsync();

        var query = _context.Post
            .Where(p => communityIds.Contains(p.IdCommunity))
            .OrderByDescending(p => p.Createdat);
        
        return query;
    } 
    
}