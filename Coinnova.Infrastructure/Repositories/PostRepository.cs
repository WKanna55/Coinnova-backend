using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Repositories;

public class PostRepository : Repository<Post>, IPostRepository
{
    private readonly ApplicationDbContext _context;
    
    public PostRepository(ApplicationDbContext _context) : base(_context)
    {
        this._context = _context;
    }

    /*
     * Refactorizado: ya no usar
     * 
     */
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
    
    public async Task<IEnumerable<Post>> GetForCommunityIds(IList<int> communityIds, int skip, int take)
    {
        var posts = await _context.Post.Where(p => 
                communityIds.Contains(p.IdCommunity)).OrderByDescending(p => p.Createdat)
            .Skip(skip).Take(take).ToListAsync();

        return posts;
    }

    public async Task<int> CountPostsAsync(IList<int> communityIds)
    {
        var totalPosts = await _context.Post.Where(p => 
            communityIds.Contains(p.IdCommunity)).CountAsync();
        return totalPosts;
    }

    public async Task<(IEnumerable<Post> Posts, int totalCount)> GetPostsByUserIdAsync(int userId, int skip, int take)
    {
        var query = _context.Post
            .AsNoTracking()
            .Where(p => p.IdUser == userId)
            .OrderByDescending(p => p.Createdat);

        var totalCount = await query.CountAsync();

        var posts = await query
            .Skip(skip)
            .Take(take)
            .Select(p => new Post {
                Id = p.Id,
                IdCommunity = p.IdCommunity,
                Createdat = p.Createdat,
                Updatedat = p.Updatedat,
                Title = p.Title,
                Textcontent = p.Textcontent,
                Imageurl = p.Imageurl,
                Likes = p.Likes,
                CommentCount = p.CommentCount,
                IdTypeNavigation = p.IdTypeNavigation,
                IdCommunityNavigation = p.IdCommunityNavigation,
                IdUserNavigation = p.IdUserNavigation,
            })
            .ToListAsync();

        return (posts, totalCount);
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

    public async Task<IOrderedQueryable<Post>> GetPostsByCommunityId(int communityId)
    {
        var query = _context.Post
            .Where(p => p.IdCommunity == communityId)
            .OrderByDescending(p => p.Createdat);
        return await Task.FromResult(query);
    }

    public async Task<Post?> LikePostById(int postId)
    {
        var post = await _context.Post.FindAsync(postId);
        if (post == null) return null;

        post.Likes = (post.Likes ?? 0) + 1;
        return post;
    }
    
} 
