using Coinnova.Application.Dtos.User;

namespace Coinnova.Application.Interfaces;

public interface IGoogleAuthService
{
    Task<GoogleUserDto?> ValidateIdTokenAsync(string idToken);
}