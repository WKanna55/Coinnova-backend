using Coinnova.Application.Dtos.User;

namespace Coinnova.Application.Dtos.Comment;

public class CommentDto : BaseCommentDto
{
    public required UserSimpleDto Author { get; set; }
    public int? ParentCommentId { get; set; }
    public int RepliesCount { get; set; }
    public int PostId { get; set; }
}