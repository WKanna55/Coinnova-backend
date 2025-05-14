using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;

namespace Coinnova.Application.Interfaces;

public interface IPostService
{
    Task<PagedResponseDto<PostsForUserIdResponseDto>> GetPostsForUserFeedById(int userId, int skip, int take);
    Task<IEnumerable<BasePostDto>> GetPostsByUserIdAsync(int userId);
    Task<PostDetailsDto> GetPostDetailsById(int postId);
    // public Task<int> CountPostsByUserIdAsync(int userId);
    Task<PagedResponseDto<PostsForCommunityDto>> GetPostsByCommunityId(int id, int skip, int take);
}