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
    
}