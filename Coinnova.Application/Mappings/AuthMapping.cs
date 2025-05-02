using Coinnova.Application.Dtos.Auth;
using Coinnova.Domain.Entities;
using Mapster;

namespace Coinnova.Application.Mappings;

public class AuthMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, LoginResponseDto>()
            .Map(dest => dest.IdUser, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.RolName, src => src.IdRoleNavigation!.Name);
    }
}