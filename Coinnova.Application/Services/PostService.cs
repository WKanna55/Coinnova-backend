using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Application.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponseDto<PostsForUserIdResponseDto>> GetPostsForUserId(int id, int skip, int take)
    {
        var query = await _unitOfWork.Posts.GetCommunitiesPostsForUserId(id);

        var totalPosts = await query.CountAsync();

        var posts = await query.Skip(skip).Take(take).ProjectToType<PostsForUserIdResponseDto>().ToListAsync();

        var hasMore = totalPosts > (skip + take);
        
        return new PagedResponseDto<PostsForUserIdResponseDto>
        {
            Items = posts,
            HasMore = hasMore,
            TotalCount = totalPosts
        };
    }
    
    public async Task<PagedResponseDto<BasePostDto>> GetPostsByUserIdAsync(int userId, int skip = 0, int take = 10)
    {
        var (posts, totalCount) = await _unitOfWork.Posts.GetPostsByUserIdAsync(userId, skip, take);
        var postDtos = posts.Adapt<IEnumerable<BasePostDto>>();

        return new PagedResponseDto<BasePostDto>
        {
            Items = postDtos,
            TotalCount = totalCount,
            HasMore = totalCount > (skip + take)
        };
    }
    
    public async Task<BasePostDto> GetPostDetailsById(int postId)
    {
        var post = await _unitOfWork.Posts.GetPostDetailsByIdAsync(postId);
        Console.WriteLine(post);
        if (post == null)
        {
            throw new KeyNotFoundException($"No existe esta publicación");
        }
        
        return post.Adapt<BasePostDto>();
    }
}