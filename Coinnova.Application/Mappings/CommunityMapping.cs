using Coinnova.Application.Dtos.Community;
using Coinnova.Domain.Entities;
using Mapster;

namespace Coinnova.Application.Mappings;

public class CommunityMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        
        // registrar aca mapeos de entidad a dto
        
        /* EJEMPLO: devolver un usuario con su rol (ya no usamos esto, sino tokens)
        config.NewConfig<User, LoginResponseDto>()
            .Map(dest => dest.IdUser, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.RolName, src => src.IdRoleNavigation!.Name);
        */
        
        config.NewConfig<Community, CommunityGetDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.NumberOfMembers, src => src.CommunityMember.Count);
        
    }
}