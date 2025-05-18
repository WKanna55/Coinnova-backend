namespace Coinnova.Application.Dtos.Post;

public class PostsForCommunityDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string TextContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public string ImageUrl { get; set; }
    public string AuthorName { get; set; }
    public string PostTypeName { get; set; }
    public int CommentsCount { get; set; }
}