namespace Coinnova.Application.Dtos.Post;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Textcontent { get; set; } = null!;
    public string? Imageurl { get; set; }
}