using Coinnova.Application.Dtos.User;

namespace Coinnova.Application.Interfaces;

public interface IUserService
{
    Task<UserGetDto?> GetUserById(int id);
}