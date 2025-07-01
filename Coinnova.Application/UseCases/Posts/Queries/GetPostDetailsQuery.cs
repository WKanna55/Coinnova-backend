using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Posts.Queries;

public record GetPostDetailsQuery(int PostId) : IRequest<BasePostDto>;

public class GetPostDetailsQueryHandler : IRequestHandler<GetPostDetailsQuery, BasePostDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPostDetailsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BasePostDto> Handle(GetPostDetailsQuery request, CancellationToken cancellationToken)
    {
        var post = await _unitOfWork.Posts.GetPostDetailsByIdAsync(request.PostId);
        
        if (post == null)
        {
            throw new KeyNotFoundException($"No existe esta publicación con ID: {request.PostId}");
        }

        return post.Adapt<BasePostDto>();
    }
} 