using System.Text.Json.Serialization;

namespace Coinnova.Application.Dtos.Comment;

public class CommentWithRepliesDto : CommentDto
{
    [JsonPropertyOrder(99)]
    public ICollection<CommentWithRepliesDto> Replies { get; set; } = new List<CommentWithRepliesDto>();
}