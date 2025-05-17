using Coinnova.Application.Dtos.User;
using Coinnova.Application.Interfaces;
using Google.Apis.Auth;

namespace Coinnova.Infrastructure.Services;

public class GoogleAuthService : IGoogleAuthService
{
    public async Task<GoogleUserDto?> ValidateIdTokenAsync(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            return new GoogleUserDto
            {
                Email = payload.Email,
                Name = payload.Name,
                Picture = payload.Picture
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}