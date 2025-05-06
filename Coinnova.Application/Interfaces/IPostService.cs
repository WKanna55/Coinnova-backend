using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;

namespace Coinnova.Application.Interfaces;

public interface IPostService
{
    Task<PagedResponseDto<PostsForUserIdResponseDto>> GetPostsForUserId(int id, int skip, int take);
}