using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Repositories;

public class PostRepository : Repository<Post>, IPostRepository
{
    private readonly ApplicationDbContext _context;

    public PostRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
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
} 