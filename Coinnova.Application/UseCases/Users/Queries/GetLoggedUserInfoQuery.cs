using Coinnova.Application.Dtos.User;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Users.Queries;

public class GetLoggedUserInfoQuery : IRequest<UserDto>
{
    public int UserId { get; set; }
}

public class GetLoggedUserInfoQueryHandler : IRequestHandler<GetLoggedUserInfoQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLoggedUserInfoQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> Handle(GetLoggedUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdWithRelations(request.UserId);
        if (user == null)
            throw new Exception("Usuario no encontrado");

        return user.Adapt<UserDto>();
    }
} 