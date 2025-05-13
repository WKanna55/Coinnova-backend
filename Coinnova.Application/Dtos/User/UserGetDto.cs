namespace Coinnova.Application.Dtos.User;

public class UserGetDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;
    
    public string? Biography { get; set; }

    public string? Imageurl { get; set; }
}