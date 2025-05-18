using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetRootCommentsAsync(int postId)
    {
        return await _context.Comment
            .Where(c => c.IdPost == postId && c.IdParentComment == null)
            .Include(c => c.IdUserNavigation)
            .Include(c => c.IdTypeNavigation)
            .OrderBy(c => c.Createdat)
            .ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetRepliesAsync(int parentCommentId)
    {
        return await _context.Comment
            .Where(c => c.IdParentComment == parentCommentId)
            .Include(c => c.IdUserNavigation)
            .Include(c => c.IdTypeNavigation)
            .OrderBy(c => c.Createdat)
            .ToListAsync();
    }
}