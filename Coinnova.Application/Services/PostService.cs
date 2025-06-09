using Coinnova.Application.Common.Files;
using Coinnova.Application.Common.Helpers;
using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Common.Models;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Domain.Interfaces.Common;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Application.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudStorageService _cloudStorage;
    private readonly FileUploadFactory _fileUploadFactory;

    public PostService(IUnitOfWork unitOfWork, ICloudStorageService cloudStorage,
        FileUploadFactory fileUploadFactory)
    {
        _unitOfWork = unitOfWork;
        _cloudStorage = cloudStorage;
        _fileUploadFactory = fileUploadFactory;
    }

    public async Task<PagedResponseDto<PostsForUserIdResponseDto>> GetPostsForUserFeedById(int userId, int skip, int take)
    {
        var query = await _unitOfWork.Posts.QueryPostsForUser(userId);

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
    
    public async Task<PagedResponseDto<PostsForCommunityDto>> GetPostsByCommunityId(int id, int skip, int take)
    {
        var query = await _unitOfWork.Posts.GetPostsByCommunityId(id);
        var totalPosts = await query.CountAsync();
        
        var posts = await query
            .Skip(skip)
            .Take(take)
            .ProjectToType<PostsForCommunityDto>()
            .ToListAsync();
        
        var hasMore = totalPosts > (skip + take);

        return new PagedResponseDto<PostsForCommunityDto>
        {
            Items = posts,
            HasMore = hasMore,
            TotalCount = totalPosts
        };
    }

    public async Task<PostDto> CreatePost(CreatePostDto createPostDto)
    {
        //var post = postDto.Adapt<Post>();

        var post = new Post
        {
            Title = createPostDto.Title,
            Textcontent = createPostDto.Textcontent,
            IdType = createPostDto.IdType,
            IdUser = createPostDto.IdUser,
            IdCommunity = createPostDto.IdCommunity
        };

        await _unitOfWork.Posts.Add(post);
        await _unitOfWork.Complete();
        
        var uploadImage = new UploadPostImageDto
        {
            PostId = post.Id,
            File = createPostDto.Image
        };
        
        var uploaded = await UploadPostImage(uploadImage);
        
        return post.Adapt<PostDto>();
    }

    public async Task<bool> UploadPostImage(UploadPostImageDto uploadPostImageDto)
    {
        if (uploadPostImageDto.File == null) 
            return false;
        
        var completeImageFile = await _fileUploadFactory.FromFormFileAsync(uploadPostImageDto.File,
            CloudinaryFolders.ForPost(uploadPostImageDto.PostId));
        if (completeImageFile == null) 
            return false;
        
        var post = await _unitOfWork.Posts.GetById(uploadPostImageDto.PostId);
        
        if (post == null) 
            return false;
        
        var imageUrl = await _cloudStorage.UploadImageAsync(completeImageFile);
        post.Imageurl = imageUrl;
        await _unitOfWork.Complete();
        return true;
    }

    public async Task<PostsForUserIdResponseDto> LikeApost(int postId)
    {
        var updated = await _unitOfWork.Posts.LikePostById(postId);
        if (updated == null) return null;
        await _unitOfWork.Complete();
        return updated.Adapt<PostsForUserIdResponseDto>();
    }
    
}