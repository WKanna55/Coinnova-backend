using Coinnova.Application.Common.Files;
using Coinnova.Application.Common.Helpers;
using Coinnova.Application.Dtos.User;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Domain.Interfaces.Common;
using MediatR;

namespace Coinnova.Application.UseCases.Users.Commands;

public class UploadUserImageCommand : IRequest<bool>
{
    public UploadUserImageDto UploadUserImageDto { get; set; } = null!;
}

public class UploadUserImageCommandHandler : IRequestHandler<UploadUserImageCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudStorageService _cloudStorage;
    private readonly FileUploadFactory _fileUploadFactory;

    public UploadUserImageCommandHandler(IUnitOfWork unitOfWork, ICloudStorageService cloudStorage,
        FileUploadFactory fileUploadFactory)
    {
        _unitOfWork = unitOfWork;
        _cloudStorage = cloudStorage;
        _fileUploadFactory = fileUploadFactory;
    }

    public async Task<bool> Handle(UploadUserImageCommand request, CancellationToken cancellationToken)
    {
        if (request.UploadUserImageDto.Image == null)
            return false;
        
        var completeImageFile = await _fileUploadFactory.FromFormFileAsync(request.UploadUserImageDto.Image,
            CloudinaryFolders.ForUser(request.UploadUserImageDto.UserId));
        if (completeImageFile == null) 
            return false;

        var user = await _unitOfWork.Users.GetById(request.UploadUserImageDto.UserId);

        if (user == null)
            return false;

        var imageUrl = await _cloudStorage.UploadImageAsync(completeImageFile);
        user.Imageurl = imageUrl;
        await _unitOfWork.Complete();

        return true;
    }
} 