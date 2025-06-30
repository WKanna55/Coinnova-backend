using Coinnova.Application.Dtos.Comment;
using Coinnova.Application.Dtos.User;
using Coinnova.Domain.Entities;
using Mapster;

namespace Coinnova.Application.Mappings;

public class CommentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Mapeo base para propiedades comunes
        config.NewConfig<Comment, BaseCommentDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Content, src => src.Content)
            .Map(dest => dest.CreatedAt, src => src.Createdat)
            .Map(dest => dest.UpdatedAt, src => src.Updatedat)
            .Map(dest => dest.Likes, src => src.Likes ?? 0)
            .Map(dest => dest.CommentTypeName, src => src.IdTypeNavigation != null ? src.IdTypeNavigation.Name : null);

        config.ForType<Comment, CommentDto>()
            .Map(dest => dest, src => src.Adapt<BaseCommentDto>())
            // .Map(dest => dest.Author, src => src.IdUserNavigation.Adapt<UserSimpleDto>())
            .Map(dest => dest.Author, src => src.IdUserNavigation != null ? src.IdUserNavigation.Adapt<UserSimpleDto>() : null)
            .Map(dest => dest.RepliesCount, src => src.ReplyCount)
            .Map(dest => dest.PostId, src => src.IdPost)
            .Map(dest => dest.ParentCommentId, src => src.IdParentComment);

        config.ForType<Comment, CommentWithRepliesDto>()
            .Map(dest => dest, src => src.Adapt<CommentDto>());
    }
}