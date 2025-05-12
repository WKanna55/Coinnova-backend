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
    
    public async Task<IEnumerable<BasePostDto>> GetPostsByUserIdAsync(int userId)
    {
        var posts = await _unitOfWork.Posts.GetPostsByUserIdAsync(userId);
        return posts.Adapt<IEnumerable<BasePostDto>>();
    }
    
    public async Task<PostDetailsDto> GetPostDetailsById(int postId)
    {
        var post = await _unitOfWork.Posts.GetPostDetailsByIdAsync(postId);
        if (post == null)
        {
            throw new KeyNotFoundException($"No existe esta publicación");
        }
        
        return post.Adapt<PostDetailsDto>();
    }

    // public async Task<PostDetailsDto> GetPostDetailsById(int postId)
    // {
    //     var post = await _unitOfWork.Posts.GetPostDetailsByIdAsync(postId);
    //     if (post == null)
    //     {
    //         throw new KeyNotFoundException($"No existe esta publicación");
    //     }
    //
    //     var postDto = post.Adapt<PostDetailsDto>();
    //
    //     var rootComments = post.Comment?
    //         .Where(c => c.IdParentComment == null)
    //         .OrderBy(c => c.Createdat)
    //         .Select(c => CommentMapperHelper.MapWithDepth(c))
    //         .ToList();
    //
    //     postDto.Comments = rootComments ?? new List<CommentWithRepliesDto>();
    //
    //     return postDto;
    // }
}