using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Coinnova.Application.Dtos.Auth;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Coinnova.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleAuthService _googleAuthService;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration config, IGoogleAuthService googleAuthService)
    {
        _unitOfWork = unitOfWork;
        _googleAuthService = googleAuthService;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginDto)
    {
        var user = await _unitOfWork.Users.GetByEmail(loginDto.Email);
        if (user == null) return null;

        if (user.AuthProvider != "Local") return null;

        var passwordInDb = user.Password;
        var isHashedPassword = passwordInDb.StartsWith("$2");

        var isPasswordValid = isHashedPassword
            ? BCrypt.Net.BCrypt.Verify(loginDto.Password, passwordInDb)
            : loginDto.Password == passwordInDb;

        if (!isPasswordValid) return null;
        
        if (!isHashedPassword)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
            await _unitOfWork.Users.Update(user);
            await _unitOfWork.Complete();
        }

        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Token = token,
            Email = user.Email
        };
    }

    public async Task<RegisterResponseDto> Register(RegisterRequestDto registerDto)
    {
        var newUser = registerDto.Adapt<User>();

        newUser.IdRole = 2; // rol estandar
        
        newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password); // hashear password
        
        await _unitOfWork.Users.Add(newUser);
        await _unitOfWork.Complete();
        
        return newUser.Adapt<RegisterResponseDto>();
    }

    public async Task<LoginResponseDto?> LoginWithGoogleAsync(string idToken)
    {
        var googleUser = await _googleAuthService.ValidateIdTokenAsync(idToken);
        
        if (googleUser == null) return null;

        var user = await _unitOfWork.Users.GetByEmail(googleUser.Email);
        
        if (user == null)
        {
            var dominio = googleUser.Email.Split('@')[1];
            var institution = await _unitOfWork.Institutions.GetByDomainAsync(dominio);
            
            user = new User
            {
                Name = googleUser.Name,
                Email = googleUser.Email,
                Password = "oauth",
                Imageurl = googleUser.Picture,
                IdRole = 2,
                IdInstitution = institution?.Id,
                AuthProvider = "Google"
            };

            await _unitOfWork.Users.Add(user);
            await _unitOfWork.Complete();
        }
        user = await _unitOfWork.Users.GetWithRoleByEmail(googleUser.Email);

        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Token = token
        };
    }
    
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.IdRoleNavigation.Name)
        };

        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!;
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}