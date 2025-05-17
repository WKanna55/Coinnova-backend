namespace Coinnova.Application.Dtos.Comment;

public class BaseCommentDto
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Likes { get; set; }
    public string? CommentTypeName { get; set; }
}
