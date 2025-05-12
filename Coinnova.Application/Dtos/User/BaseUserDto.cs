namespace Coinnova.Application.Dtos.User;

public class BaseUserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Biography { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? CreatedAt { get; set; }
}