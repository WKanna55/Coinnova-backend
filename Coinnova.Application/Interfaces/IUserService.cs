using Coinnova.Application.Dtos.User;
using Coinnova.Application.Dtos.User.HttpMethods;
using Coinnova.Domain.Entities;

namespace Coinnova.Application.Interfaces;

public interface IUserService
{
    Task<UserGetDto?> GetUserById(int id);
    Task<UserDto> GetUserInfoById(int id);
    Task<UpdateUserResponseDto> UpdateUserAsync(int userId, UpdateUserRequestDto userRequestDto);
    Task<IEnumerable<UserSimpleDto>> GetFirstCommunityMembers(int communityId);
    Task<UserDto> GetLoggedUserInfo(int userId);
    Task<bool> UploadProfileImage(UploadUserImageDto uploadUserImageDto);
}