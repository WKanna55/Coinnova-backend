using Coinnova.Application.Dtos.User;
using Coinnova.Application.Dtos.User.HttpMethods;
using Coinnova.Domain.Entities;
using Mapster;

namespace Coinnova.Application.Mappings;

public class UserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Biography, src => src.Biography)
            .Map(dest => dest.ImageUrl, src => src.Imageurl)
            .Map(dest => dest.Role, src => src.IdRoleNavigation.Name)
            .Map(dest => dest.CreatedAt, src => src.Createdat)
            .Map(dest => dest.InstitutionId, src => src.IdInstitution)
            .Map(dest => dest.InstitutionName, src => src.IdInstitutionNavigation.Name);

        config.NewConfig<User, UpdateUserResponseDto>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Biography, src => src.Biography)
            .Map(dest => dest.ImageUrl, src => src.Imageurl);

        config.NewConfig<User, UserSimpleDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.ImageUrl, src => src.Imageurl);

        // ---------- PATCH parcial(no mapea nulos): DTO → entidad ----------
        config.NewConfig<UpdateUserRequestDto, User>()     // dirección correcta
            .IgnoreNullValues(true)                     // NO sobrescribe con null
            .Map(dest => dest.Name,       src => src.Name)
            .Map(dest => dest.Biography,  src => src.Biography);
    }
}