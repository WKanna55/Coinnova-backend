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

    public async Task<(IEnumerable<Post> Posts, int TotalCount)> GetPostsByUserIdAsync(int userId, int skip, int take)
    {
        var query = _context.Post
            .Where(p => p.IdUser == userId)
            .OrderByDescending(p => p.Createdat);

        var totalCount = await query.CountAsync();

        var posts = await query
            .Skip(skip)
            .Take(take)
            .Select(p => new
            {
                Post = p,
                Type = p.IdTypeNavigation,
                Community = p.IdCommunityNavigation,
                Author = p.IdUserNavigation,
                CommentCount = _context.Comment.Count(c => c.IdPost == p.Id)
            }).ToListAsync();
        
        var result = posts.Select(p =>
        {
            p.Post.IdTypeNavigation = p.Type;
            p.Post.IdCommunityNavigation = p.Community;
            p.Post.IdUserNavigation = p.Author;
            p.Post.CommentCount = p.CommentCount;
            return p.Post;
        }).ToList();

        return (result, totalCount);
    }

    public async Task<Post?> GetPostDetailsByIdAsync(int postId)
    {
        return await _context.Post
            .Include(p => p.IdCommunityNavigation)
            .Include(p => p.IdUserNavigation)
            .Include(p => p.IdTypeNavigation)
            .Include(p => p.Comment)
            .FirstOrDefaultAsync(p => p.Id == postId);
    }
} 
