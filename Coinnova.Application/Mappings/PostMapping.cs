using Coinnova.Application.Dtos.Comment;
using Coinnova.Application.Dtos.Community;
using Coinnova.Application.Dtos.Post;
using Coinnova.Application.Dtos.User;
using Coinnova.Domain.Entities;
using Mapster;

namespace Coinnova.Application.Mappings;

public class PostMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Post, BasePostDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.TextContent, src => src.Textcontent)
            .Map(dest => dest.CreatedAt, src => src.Createdat)
            .Map(dest => dest.UpdatedAt, src => src.Updatedat)
            .Map(dest => dest.Likes, src => src.Likes ?? 0)
            .Map(dest => dest.ImageUrl, src => src.Imageurl ?? string.Empty)
            .Map(dest => dest.PostTypeName, src => src.IdTypeNavigation.Name)
            .Map(dest => dest.CommentCount, src => src.CommentCount)
            .Map(dest => dest.Author, src => src.IdUserNavigation)
            .Map(dest => dest.Community, src => src.IdCommunityNavigation);
    }
}