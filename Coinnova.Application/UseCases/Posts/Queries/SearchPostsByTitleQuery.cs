using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Posts.Queries;

public record SearchPostsByTitleQuery(string Title, int Skip, int Take) : IRequest<PagedResponseDto<BasePostDto>>;

public class SearchPostsByTitleQueryHandler : IRequestHandler<SearchPostsByTitleQuery, PagedResponseDto<BasePostDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchPostsByTitleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponseDto<BasePostDto>> Handle(SearchPostsByTitleQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return new PagedResponseDto<BasePostDto>
            {
                Items = Enumerable.Empty<BasePostDto>(),
                HasMore = false,
                TotalCount = 0
            };
        }

        var (posts, totalCount) = await _unitOfWork.Posts.SearchPostsByTitleAsync(request.Title, request.Skip, request.Take);
        var postDtos = posts.Adapt<IEnumerable<BasePostDto>>();
        var hasMore = totalCount > (request.Skip + request.Take);

        return new PagedResponseDto<BasePostDto>
        {
            Items = postDtos,
            TotalCount = totalCount,
            HasMore = hasMore
        };
    }
} 