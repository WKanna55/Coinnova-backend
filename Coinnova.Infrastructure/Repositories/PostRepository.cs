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

    public async Task<IOrderedQueryable<Post>> QueryPostsForUser(int userId)
    {
        var communityIds = await _context.CommunityMember
            .Where(cm => cm.IdUser == userId)
            .Select(cm => cm.IdCommunity)
            .ToListAsync();

        var query = _context.Post
            .Where(p => communityIds.Contains(p.IdCommunity))
            .OrderByDescending(p => p.Createdat);
        
        return query;
    } 
    
    public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId)
    {
        return await _context.Post
            .Where(p => p.IdUser == userId)
            .Include(p => p.IdTypeNavigation)
            .Include(p => p.IdCommunityNavigation)
            .Include(p => p.Comment)
            .ToArrayAsync();
    }

    public async Task<Post?> GetPostDetailsByIdAsync(int postId)
    {
        return await _context.Post
            .Include(p => p.IdCommunityNavigation)
            .Include(p => p.IdUserNavigation)
            .Include(p => p.IdTypeNavigation)
            .Include(p => p.Comment)
                .ThenInclude(c => c.IdUserNavigation)
            .Include(p => p.Comment)
                .ThenInclude(c => c.InverseIdParentCommentNavigation)
            .FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<IOrderedQueryable<Post>> GetPostsByCommunityId(int communityId)
    {
        var query = _context.Post
            .Where(p => p.IdCommunity == communityId)
            .OrderByDescending(p => p.Createdat);
        return await Task.FromResult(query);
    }
} 
