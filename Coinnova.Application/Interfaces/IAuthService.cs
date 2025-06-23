using Coinnova.Application.Dtos.Auth;
using Coinnova.Application.Dtos.User;

namespace Coinnova.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> Login(LoginRequestDto loginDto);
    Task<RegisterResponseDto> Register(RegisterRequestDto registerDto);
    Task<LoginResponseDto?> LoginWithGoogleAsync(string idToken);
}