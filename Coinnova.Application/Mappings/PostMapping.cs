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
            .Map(dest => dest.Title,          src => src.Title)
            .Map(dest => dest.TextContent,    src => src.Textcontent)
            .Map(dest => dest.CreatedAt,      src => src.Createdat)
            .Map(dest => dest.UpdatedAt,      src => src.Updatedat)
            .Map(dest => dest.Likes,          src => src.Likes ?? 0)
            .Map(dest => dest.ImageUrl,       src => src.Imageurl ?? string.Empty)
            .Map(dest => dest.PostTypeName,   src => src.IdTypeNavigation.Name)
            .Map(dest => dest.CommentsCount,  src => src.Comment.Count);

        config.ForType<Post, PostDetailsDto>()
            .Map(dest => dest, src => src.Adapt<BasePostDto>())
            .Map(dest => dest.Author, src => src.IdUserNavigation.Adapt<UserSimpleDto>())
            .Map(dest => dest.Community, src => src.IdCommunityNavigation.Adapt<CommunitySimpleDto>())
            .Map(dest => dest.Comments, src => src.Comment
                .Where(c => c.IdParentComment == null)
                .Adapt<ICollection<CommentWithRepliesDto>>());

        config.NewConfig<Post, PostsForCommunityDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.TextContent, src => src.Textcontent)
            .Map(dest => dest.CreatedAt, src => src.Createdat)
            .Map(dest => dest.Likes, src => src.Likes)
            .Map(dest => dest.ImageUrl, src => src.Imageurl)
            .Map(dest => dest.AuthorName, src => src.IdUserNavigation.Name)
            .Map(dest => dest.PostTypeName, src => src.IdTypeNavigation.Name)
            .Map(dest => dest.CommentsCount, src => src.Comment.Count);
    }
}