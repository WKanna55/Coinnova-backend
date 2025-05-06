using Coinnova.Application.Dtos.User;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;

namespace Coinnova.Application.Services;

public class UserService : IUserService
{

    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserGetDto?> GetUserById(int id)
    {
        var user = await _unitOfWork.Repository<User>().GetById(id);

        if (user == null) 
            return null;

        return user.Adapt<UserGetDto>();
    }


}