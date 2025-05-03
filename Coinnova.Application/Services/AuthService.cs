using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Coinnova.Application.Dtos.Auth;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Coinnova.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginDto)
    {
        var user = await _unitOfWork.Users.GetByEmail(loginDto.Email);
        if (user == null)
            return null!;

        var passwordInDb = user.Password;
        var isHashedPassword = passwordInDb.StartsWith("$2");

        if (isHashedPassword && !BCrypt.Net.BCrypt.Verify(loginDto.Password, passwordInDb))
            return null;

        if (!isHashedPassword && loginDto.Password != passwordInDb)
            return null;

        if (!isHashedPassword)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
            await _unitOfWork.Users.Update(user);
            await _unitOfWork.Complete();
        }

        // Generar token
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

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
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
    
    
    
}