using Coinnova.Application.Dtos.Comment;
using Coinnova.Domain.Entities;
using Mapster;

namespace Coinnova.Application.Mappings;

public class CommentMapperHelper
{
    public static CommentWithRepliesDto MapWithDepth(Comment comment, int currentDepth = 0, int maxDepth = 2)
    {
        var dto = comment.Adapt<CommentWithRepliesDto>();

        if (currentDepth < maxDepth && comment.InverseIdParentCommentNavigation?.Any() == true)
        {
            dto.Replies = comment.InverseIdParentCommentNavigation
                .OrderBy(c => c.Createdat)
                .Select(child => MapWithDepth(child, currentDepth + 1, maxDepth))
                .ToList();
        }
        else
        {
            dto.Replies = new List<CommentWithRepliesDto>();
        }

        return dto;
    }
}