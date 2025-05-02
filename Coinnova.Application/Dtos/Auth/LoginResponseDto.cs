namespace Coinnova.Application.Dtos.Auth;

public class LoginResponseDto
{
    public int IdUser { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string RolName { get; set; } = null!;
}