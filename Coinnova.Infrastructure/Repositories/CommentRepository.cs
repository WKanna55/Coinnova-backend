using System.ComponentModel.DataAnnotations.Schema;
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
            .Select(c => new Comment
            {
                Id = c.Id,
                Content = c.Content,
                Createdat = c.Createdat,
                Updatedat = c.Updatedat,
                Likes = c.Likes,
                IdTypeNavigation = c.IdTypeNavigation,
                IdUserNavigation = c.IdUserNavigation,
                ReplyCount = c.InverseIdParentCommentNavigation.Count(),
                IdParentComment = c.IdParentComment
            })
            .OrderBy(c => c.Createdat)
            .ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetRepliesAsync(int parentCommentId)
    {
        return await _context.Comment
            .Where(c => c.IdParentComment == parentCommentId)
            .Include(c => c.IdUserNavigation)
            .Include(c => c.IdTypeNavigation)
            .Select(c => new Comment
            {
                Id = c.Id,
                Content = c.Content,
                Createdat = c.Createdat,
                Updatedat = c.Updatedat,
                Likes = c.Likes,
                IdTypeNavigation = c.IdTypeNavigation,
                IdUserNavigation = c.IdUserNavigation,
                ReplyCount = c.InverseIdParentCommentNavigation.Count(),
                IdParentComment = c.IdParentComment
            })
            .OrderBy(c => c.Createdat)
            .ToListAsync();
    }

    public async Task<Comment?> GetCommentById(int commentId)
    {
        return await _context.Comment
            .AsNoTracking()
            .Where(c => c.Id == commentId)
            .Include(c => c.IdUserNavigation)
            .Include(c => c.IdTypeNavigation)
            .Include(c => c.InverseIdParentCommentNavigation)
            .Select(c => new Comment
            {
                Id = c.Id,
                Content = c.Content,
                Createdat = c.Createdat,
                Updatedat = c.Updatedat,
                Likes = c.Likes,
                IdTypeNavigation = c.IdTypeNavigation,
                IdUserNavigation = c.IdUserNavigation,
                ReplyCount = c.InverseIdParentCommentNavigation.Count(),
                InverseIdParentCommentNavigation = c.InverseIdParentCommentNavigation,
                IdParentComment = c.IdParentComment
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetCommentDepthAsync(int commentId)
    {
        var comment = await _context.Comment
            .Where(c => c.Id == commentId)
            .Select(c => new { c.Id, c.IdParentComment })
            .FirstOrDefaultAsync();

        if (comment == null) return -1;

        int depth = 0;
        int? currentParentId = comment.IdParentComment;

        while (currentParentId.HasValue)
        {
            depth++;
            var parent = await _context.Comment
                .Where(c => c.Id == currentParentId.Value)
                .Select(c => new { c.IdParentComment })
                .FirstOrDefaultAsync();
            if (parent == null) break;
            currentParentId = parent.IdParentComment;
        }

        return depth;
    }
}