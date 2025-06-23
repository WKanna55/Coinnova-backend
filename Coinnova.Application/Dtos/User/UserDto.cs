namespace Coinnova.Application.Dtos.User;

public class UserDto : BaseUserDto
{
    public string? Role { get; set; }
    public int? InstitutionId { get; set; }
    public string? InstitutionName { get; set; }
}