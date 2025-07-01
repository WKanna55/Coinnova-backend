using Coinnova.Application.Common.Files;
using Coinnova.Application.Common.Helpers;
using Coinnova.Application.Dtos.User;
using Coinnova.Application.Dtos.User.HttpMethods;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Domain.Interfaces.Common;
using MapsterMapper;
using MediatR;
using Mapster;

namespace Coinnova.Application.UseCases.Users.Commands;

public class UpdateUserCommand : IRequest<UpdateUserResponseDto>
{
    public int UserId { get; set; }
    public UpdateUserRequestDto UserRequestDto { get; set; } = null!;
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICloudStorageService _cloudStorage;
    private readonly FileUploadFactory _fileUploadFactory;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, 
        ICloudStorageService cloudStorage, FileUploadFactory fileUploadFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cloudStorage = cloudStorage;
        _fileUploadFactory = fileUploadFactory;
    }

    public async Task<UpdateUserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetById(request.UserId);
        if (user == null) throw new KeyNotFoundException();

        // actualiza name y biography
        request.UserRequestDto.Adapt(user);
        await _unitOfWork.Users.Update(user);
        await _unitOfWork.Complete();
        
        // si hay imagen, actualiza
        if (request.UserRequestDto.Image != null)
        {
            var uploadImage = new UploadUserImageDto
            {
                UserId = request.UserId,
                Image = request.UserRequestDto.Image
            };
            await UploadProfileImage(uploadImage);
        }

        return _mapper.Map<UpdateUserResponseDto>(user);
    }

    private async Task<bool> UploadProfileImage(UploadUserImageDto uploadUserImageDto)
    {
        if (uploadUserImageDto.Image == null)
            return false;
        
        var completeImageFile = await _fileUploadFactory.FromFormFileAsync(uploadUserImageDto.Image,
            CloudinaryFolders.ForUser(uploadUserImageDto.UserId));
        if (completeImageFile == null) 
            return false;

        var user = await _unitOfWork.Users.GetById(uploadUserImageDto.UserId);

        if (user == null)
            return false;

        var imageUrl = await _cloudStorage.UploadImageAsync(completeImageFile);
        user.Imageurl = imageUrl;
        await _unitOfWork.Complete();

        return true;
    }
} 