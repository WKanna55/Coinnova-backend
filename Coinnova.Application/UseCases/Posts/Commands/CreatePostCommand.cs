using Coinnova.Application.Common.Files;
using Coinnova.Application.Common.Helpers;
using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Domain.Interfaces.Common;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Posts.Commands;

public record CreatePostCommand(CreatePostDto CreatePostDto) : IRequest<PostDto>;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudStorageService _cloudStorage;
    private readonly FileUploadFactory _fileUploadFactory;

    public CreatePostCommandHandler(IUnitOfWork unitOfWork, ICloudStorageService cloudStorage, FileUploadFactory fileUploadFactory)
    {
        _unitOfWork = unitOfWork;
        _cloudStorage = cloudStorage;
        _fileUploadFactory = fileUploadFactory;
    }

    public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post
        {
            Title = request.CreatePostDto.Title,
            Textcontent = request.CreatePostDto.Textcontent,
            IdType = request.CreatePostDto.IdType,
            IdUser = request.CreatePostDto.IdUser,
            IdCommunity = request.CreatePostDto.IdCommunity
        };

        await _unitOfWork.Posts.Add(post);
        await _unitOfWork.Complete();

        // Subir imagen si existe
        if (request.CreatePostDto.Image != null)
        {
            await UploadPostImage(post.Id, request.CreatePostDto.Image);
        }

        return post.Adapt<PostDto>();
    }

    private async Task<bool> UploadPostImage(int postId, Microsoft.AspNetCore.Http.IFormFile file)
    {
        var completeImageFile = await _fileUploadFactory.FromFormFileAsync(file,
            CloudinaryFolders.ForPost(postId));
        
        if (completeImageFile == null)
            return false;

        var post = await _unitOfWork.Posts.GetById(postId);
        if (post == null)
            return false;

        var imageUrl = await _cloudStorage.UploadImageAsync(completeImageFile);
        post.Imageurl = imageUrl;
        await _unitOfWork.Complete();
        return true;
    }
} 