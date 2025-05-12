namespace Coinnova.Application.Dtos.User;

public class UserSimpleDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
}