namespace Coinnova.Application.Dtos.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
}