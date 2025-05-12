namespace Coinnova.Application.Dtos.User;

public class UpdateUserResponseDto
{
    public required string Name { get; set; }
    public string? Biography { get; set; }
    public string? ImageUrl { get; set; }
}