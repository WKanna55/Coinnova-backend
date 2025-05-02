using Coinnova.Application.Dtos.Auth;
using Coinnova.Application.Interfaces;
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
        if (user == null || user.Password != loginDto.Password)
            return default!;

        return user.Adapt<LoginResponseDto>(); // ← ya aplica el mapeo personalizado
    }
    
    
}