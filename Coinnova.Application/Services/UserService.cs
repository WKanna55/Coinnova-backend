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

    public async Task<UserGetDto?> GetUserById(int id)
    {
        var user = await _unitOfWork.Repository<User>().GetById(id);

        if (user == null) 
            return null;

        return user.Adapt<UserGetDto>();
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

    public async Task<IEnumerable<UserSimpleDto>> GetFirstCommunityMembers(int communityId)
    {
        var users = await _unitOfWork.Users.GetFirstMembersByCommunityId(communityId, 6);
        return users.Adapt<IEnumerable<UserSimpleDto>>();
    }
    
    public async Task<UserDto> GetLoggedUserInfo(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdWithRelations(userId);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        return user.Adapt<UserDto>();
    }
}