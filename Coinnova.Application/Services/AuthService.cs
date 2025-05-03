using Coinnova.Application.Dtos.Auth;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;

namespace Coinnova.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginDto)
    {
        var user = await _unitOfWork.Users.GetByEmail(loginDto.Email);
        if (user == null)
            return null!;

        var passwordInDb = user.Password;

        // Primero verificar como hash
        bool isHashedPassword = passwordInDb.StartsWith("$2");
    
        if (isHashedPassword)
        {
            if (BCrypt.Net.BCrypt.Verify(loginDto.Password, passwordInDb))
                return user.Adapt<LoginResponseDto>();
            
            return null;
        }

        // Segundo si no era hash, verificar si coincide como texto plano
        if (loginDto.Password == passwordInDb)
        {
            // Migrar el password a hash
            var hashed = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
            user.Password = hashed;
            await _unitOfWork.Users.Update(user);
            await _unitOfWork.Complete();

            return user.Adapt<LoginResponseDto>();
        }

        return null;
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