using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using MediatR;

namespace Coinnova.Application.UseCases.Comments.Commands;

public class CreateCommentCommand : IRequest<Comment>
{
    public string Content  { get; set; }
    public int? IdType { get; set; }
    public int IdUser { get; set; }
    public int IdPost { get; set; }
    public int? IdParentComment { get; set; }
}

internal sealed class CreateCommentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCommentCommand, Comment>
{
    public async Task<Comment> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        request.IdParentComment = request.IdParentComment > 0 ? request.IdParentComment : null;
        if (request.IdParentComment.HasValue)
            request.IdType = null;
        
        var newComment = new Comment
        {
            Content = request.Content,
            IdType = request.IdType,
            IdUser = request.IdUser,
            IdPost = request.IdPost,
            IdParentComment = request.IdParentComment
        };
        
        await unitOfWork.Repository<Comment>().Add(newComment);
        await unitOfWork.Complete();
        
        return newComment;
    }
}