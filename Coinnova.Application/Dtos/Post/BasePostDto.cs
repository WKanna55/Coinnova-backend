using Coinnova.Application.Dtos.Community;
using Coinnova.Application.Dtos.User;

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
    public int CommentCount { get; set; }
    public required UserSimpleDto Author { get; set; }
    public required CommunitySimpleDto Community { get; set; }
}


