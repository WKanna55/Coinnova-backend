using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Coinnova.Application.Dtos.Auth;
using Coinnova.Application.Dtos.User;
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
            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.IdRoleNavigation.Name,
                InstitutionId = user.IdInstitution,
                InstitutionName = user.IdInstitutionNavigation?.Name
            }
        };
    }

    public async Task<RegisterResponseDto> Register(RegisterRequestDto registerDto)
    {
        var newUser = registerDto.Adapt<User>();

        newUser.IdRole = 2; // rol estandar
        
        newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password); // hashear password
        
        await _unitOfWork.Users.Add(newUser);
        await _unitOfWork.Complete();
        
        var createdUser = await _unitOfWork.Users.GetByEmail(newUser.Email);
        if (createdUser == null) throw new Exception("Error al recuperar usuario recién creado");

        var token = GenerateJwtToken(createdUser);
        
        return new RegisterResponseDto
        {
            Token = token,
            User = createdUser.Adapt<UserDto>()
        };
    }

    public async Task<LoginResponseDto?> LoginWithGoogleAsync(string idToken)
    {
        var googleUser = await _googleAuthService.ValidateIdTokenAsync(idToken);
        
        if (googleUser == null) return null;

        var user = await _unitOfWork.Users.GetByEmail(googleUser.Email);
        
        if (user == null)
        {
            var dominio = googleUser.Email.Split('@')[1];
            var institution = await _unitOfWork.InstitutionRepository.GetByDomainAsync(dominio);
            
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
        
        // Obtener el usuario  con sus relaciones
        user = await _unitOfWork.Users.GetWithRoleByEmail(googleUser.Email);
        if (user == null)
            throw new Exception("Error al recuperar el usuario");
        
        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Token = token,
            User = user.Adapt<UserDto>()
        };
    }
    
    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.IdRoleNavigation.Name),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!;
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;
        var jwtLifetimeDays = int.Parse(Environment.GetEnvironmentVariable("JWT_LIFETIME_DAYS")!);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddDays(jwtLifetimeDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}