using Coinnova.Application.Dtos.User;

namespace Coinnova.Application.Dtos.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}