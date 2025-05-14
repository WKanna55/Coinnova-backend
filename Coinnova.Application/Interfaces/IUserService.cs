using Coinnova.Application.Dtos.User;
using Coinnova.Domain.Entities;

namespace Coinnova.Application.Interfaces;

public interface IUserService
{
    Task<UserGetDto?> GetUserById(int id);
    Task<UserDto> GetUserInfoById(int id);
    Task<UpdateUserResponseDto> UpdateUserAsync(int userId, UpdateUserRequestDto dto);
    Task<IEnumerable<UserSimpleDto>> GetFirstCommunityMembers(int communityId);
}