using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Posts.Queries;

public record GetPostsByUserIdQuery(int UserId, int Skip = 0, int Take = 10) : IRequest<PagedResponseDto<BasePostDto>>;

public class GetPostsByUserIdQueryHandler : IRequestHandler<GetPostsByUserIdQuery, PagedResponseDto<BasePostDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPostsByUserIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponseDto<BasePostDto>> Handle(GetPostsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var (posts, totalCount) = await _unitOfWork.Posts.GetPostsByUserIdAsync(request.UserId, request.Skip, request.Take);
        var postDtos = posts.Adapt<IEnumerable<BasePostDto>>();

        return new PagedResponseDto<BasePostDto>
        {
            Items = postDtos,
            TotalCount = totalCount,
            HasMore = totalCount > (request.Skip + request.Take)
        };
    }
} 