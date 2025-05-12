namespace Coinnova.Application.Dtos.Post;

public class PostsForUserIdResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Textcontent { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public int? Likes { get; set; }

    public int IdType { get; set; }

    public string? Imageurl { get; set; }

    public int IdUser { get; set; }

    public int IdCommunity { get; set; }
}