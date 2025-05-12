using Coinnova.Application.Dtos.Post;

namespace Coinnova.Application.Interfaces;

public interface IPostService
{
    public Task<IEnumerable<BasePostDto>> GetPostsByUserIdAsync(int userId);

    public Task<PostDetailsDto> GetPostDetailsById(int postId);
    // public Task<int> CountPostsByUserIdAsync(int userId);
}