using Coinnova.Application.Dtos.User;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MapsterMapper;

namespace Coinnova.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto> GetUserInfoById(int userId)
    {
        var user = await _unitOfWork.Repository<User>().GetById(userId);
        return user.Adapt<UserDto>();
    }

    public async Task<UpdateUserResponseDto> UpdateUserAsync(int userId, UpdateUserRequestDto dto)
    {
        var user = await _unitOfWork.Users.GetById(userId);
        if (user == null) throw new KeyNotFoundException();
        
        _mapper.Map(dto, user);

        await _unitOfWork.Users.Update(user);
        await _unitOfWork.Complete();

        return _mapper.Map<UpdateUserResponseDto>(user);
    }
}