namespace Coinnova.Application.Dtos.User;

public class GoogleUserDto
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Picture { get; set; }
}