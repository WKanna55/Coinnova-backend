using Coinnova.Application.Dtos.Auth;

namespace Coinnova.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> Login(LoginRequestDto loginDto);
    Task<RegisterResponseDto> Register(RegisterRequestDto registerDto);
}