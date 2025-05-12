namespace Coinnova.Application.Dtos.User;

public class UserDto : BaseUserDto
{
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public int? InstitutionId { get; set; }
    public string? InstitutionName { get; set; }
}