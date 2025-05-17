using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;

namespace Coinnova.Application.Interfaces;

public interface IPostService
{
    Task<PagedResponseDto<PostsForUserIdResponseDto>> GetPostsForUserId(int id, int skip, int take);
    Task<PagedResponseDto<BasePostDto>> GetPostsByUserIdAsync(int userId, int skip = 0, int take = 10);
    Task<BasePostDto> GetPostDetailsById(int postId);
    Task<PagedResponseDto<PostsForUserIdResponseDto>> GetPostsForUserFeedById(int userId, int skip, int take);
    Task<PagedResponseDto<PostsForCommunityDto>> GetPostsByCommunityId(int id, int skip, int take);
    // public Task<int> CountPostsByUserIdAsync(int userId);
}