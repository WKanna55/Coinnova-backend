using Coinnova.Application.Dtos.Community;
using Coinnova.Domain.Entities;
using Mapster;

namespace Coinnova.Application.Mappings;

public class CommunityMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Community, CommunityBaseDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);

        config.ForType<Community, CommunityUsingBaseDto>()
            .Map(dest => dest, src => src.Adapt<CommunityBaseDto>())
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.ImageUrl, src => src.Imageurl)
            .Map(dest => dest.MemberCount, src => src.MemberCount);
    }
}