namespace Coinnova.Application.Dtos.Post;

public class BasePostDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string TextContent { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Likes { get; set; }
    public required string PostTypeName { get; set; }
    public required string ImageUrl { get; set; }
    public int CommentsCount { get; set; }
}


