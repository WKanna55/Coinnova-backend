using Coinnova.Application.Dtos.User;

namespace Coinnova.Application.Dtos.Auth;

public class RegisterResponseDto
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}