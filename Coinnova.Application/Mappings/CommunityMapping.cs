using Coinnova.Application.Dtos.Community;
using Coinnova.Domain.Entities;
using Mapster;

namespace Coinnova.Application.Mappings;

public class CommunityMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Community, CommunitySimpleDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);
    }
}