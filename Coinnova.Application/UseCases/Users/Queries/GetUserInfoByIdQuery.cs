using Coinnova.Application.Dtos.User;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Users.Queries;

public class GetUserInfoByIdQuery : IRequest<UserDto>
{
    public int UserId { get; set; }
}

public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserInfoByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().GetById(request.UserId);
        return user.Adapt<UserDto>();
    }
} 