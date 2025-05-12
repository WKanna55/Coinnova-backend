using Coinnova.Application.Dtos.Comment;
using Coinnova.Application.Dtos.Community;
using Coinnova.Application.Dtos.User;

namespace Coinnova.Application.Dtos.Post;

public class PostDetailsDto : BasePostDto
{
    public required UserSimpleDto Author { get; set; }
    public required CommunitySimpleDto Community { get; set; }
    public ICollection<CommentWithRepliesDto> Comments { get; set; } = new List<CommentWithRepliesDto>();
}