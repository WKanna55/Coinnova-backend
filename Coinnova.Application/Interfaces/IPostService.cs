using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Coinnova.Application.Interfaces;

public interface IPostService
{
    Task<PagedResponseDto<BasePostDto>> GetPostsByUserIdAsync(int userId, int skip, int take);
    Task<BasePostDto> GetPostDetailsById(int postId);
    
    // en desuso
    Task<PagedResponseDto<PostsForUserIdResponseDto>> GetPostsForUserFeedById(int userId, int skip, int take);
    
    Task<PagedResponseDto<BasePostDto>> GetPostsByCommunityId(int id, int skip, int take);
    // public Task<int> CountPostsByUserIdAsync(int userId);
    Task<PostDto> CreatePost(CreatePostDto createPostDto);
    Task<bool> UploadPostImage(UploadPostImageDto uploadPostImageDto);
    Task<PostsForUserIdResponseDto> LikeApost(int postId);

    Task<PagedResponseDto<BasePostDto>> GetAllForUserFeedById(int userId, int skip, int take);
    Task<PagedResponseDto<PostsForUserIdResponseDto>> GetInstitutionForUserFeedById(int userId, int skip, int take);
}