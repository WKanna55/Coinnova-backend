using Coinnova.Application.Dtos.User;
using Coinnova.Domain.Entities;

namespace Coinnova.Application.Interfaces;

public interface IUserService
{
    public Task<UserDto> GetUserInfoById(int id);
    public Task<UpdateUserResponseDto> UpdateUserAsync(int userId, UpdateUserRequestDto dto);
}