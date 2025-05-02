namespace Coinnova.Application.Dtos.Auth;

public class RegisterRequestDto
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}