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

    public async Task<IEnumerable<(Comment Comment, int ReplyCount)>> GetRootCommentsWithReplyCountAsync(int postId)
    {
        var commentsWithData = await _context.Comment
            .Where(c => c.IdPost == postId && c.IdParentComment == null)
            .Include(c => c.IdUserNavigation)
            .Include(c => c.IdTypeNavigation)
            .Select(c => new
            {
                CommentEntity = c,
                CountOfReplies = c.InverseIdParentCommentNavigation.Count()
            })
            .OrderBy(c => c.CommentEntity.Createdat)
            .ToListAsync();

        return commentsWithData.Select(data => (data.CommentEntity, data.CountOfReplies));
    }

    public async Task<IEnumerable<(Comment Comment, int ReplyCount)>> GetRepliesWithReplyCountAsync(int parentCommentId)
    {
        var repliesWithData = await _context.Comment
            .Where(c => c.IdParentComment == parentCommentId)
            .Include(c => c.IdUserNavigation)
            .Include(c => c.IdTypeNavigation)
            .Select(c => new
            {
                CommentEntity = c,
                CountOfReplies = c.InverseIdParentCommentNavigation.Count()
            })
            .OrderBy(c => c.CommentEntity.Createdat)
            .ToListAsync();

        return repliesWithData.Select(data => (data.CommentEntity, data.CountOfReplies));
    }
    
    public async Task<int> CountRepliesByCommentIdAsync(int commentId)
    {
        return await _context.Comment
            .CountAsync(c => c.IdParentComment == commentId);
    }
}