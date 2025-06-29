using Coinnova.Application.Common.Files;
using Coinnova.Application.Common.Helpers;
using Coinnova.Application.Dtos.User;
using Coinnova.Application.Dtos.User.HttpMethods;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Domain.Interfaces.Common;
using Mapster;
using MapsterMapper;

namespace Coinnova.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICloudStorageService _cloudStorage;
    private readonly FileUploadFactory _fileUploadFactory;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorage,
        FileUploadFactory fileUploadFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cloudStorage = cloudStorage;
        _fileUploadFactory = fileUploadFactory;
    }

    public async Task<UserGetDto?> GetUserById(int id)
    {
        var user = await _unitOfWork.Repository<User>().GetById(id);

        if (user == null) 
            return null;

        return user.Adapt<UserGetDto>();
    }

    public async Task<UserDto> GetUserInfoById(int userId)
    {
        var user = await _unitOfWork.Repository<User>().GetById(userId);
        return user.Adapt<UserDto>();
    }

    public async Task<UpdateUserResponseDto> UpdateUserAsync(int userId, UpdateUserRequestDto userRequestDto)
    {
        
        var user = await _unitOfWork.Users.GetById(userId);
        if (user == null) throw new KeyNotFoundException();

        // actualiza name y biography
        //_mapper.Map(userRequestDto, user);
        userRequestDto.Adapt(user);
        await _unitOfWork.Users.Update(user);
        await _unitOfWork.Complete();
        
        // si hay imagen, actualiza
        if (userRequestDto.Image != null)
        {
            var uploadImage = new UploadUserImageDto
            {
                UserId = userId,
                Image = userRequestDto.Image
            };
            var uploaded = await UploadProfileImage(uploadImage);
        }

        return _mapper.Map<UpdateUserResponseDto>(user);
    }

    public async Task<IEnumerable<UserSimpleDto>> GetFirstCommunityMembers(int communityId)
    {
        var users = await _unitOfWork.Users.GetFirstMembersByCommunityId(communityId, 6);
        return users.Adapt<IEnumerable<UserSimpleDto>>();
    }
    
    public async Task<UserDto> GetLoggedUserInfo(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdWithRelations(userId);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        return user.Adapt<UserDto>();
    }

    public async Task<bool> UploadProfileImage(UploadUserImageDto uploadUserImageDto)
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
    
    public async Task<UserDto> GetDetailedById(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdWithRelations(userId);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        return user.Adapt<UserDto>();
    }
    
}