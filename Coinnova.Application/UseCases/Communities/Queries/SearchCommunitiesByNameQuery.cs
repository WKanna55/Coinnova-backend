using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Community;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Communities.Queries;

public record SearchCommunitiesByNameQuery(string Name, int Skip, int Take) : IRequest<PagedResponseDto<CommunityUsingBaseDto>>;

public class SearchCommunitiesByNameQueryHandler : IRequestHandler<SearchCommunitiesByNameQuery, PagedResponseDto<CommunityUsingBaseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchCommunitiesByNameQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponseDto<CommunityUsingBaseDto>> Handle(SearchCommunitiesByNameQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return new PagedResponseDto<CommunityUsingBaseDto>
            {
                Items = Enumerable.Empty<CommunityUsingBaseDto>(),
                HasMore = false,
                TotalCount = 0
            };
        
        var (communities, totalCount) = await _unitOfWork.Communities.SearchCommunitiesByName(request.Name, request.Skip, request.Take);
        var communityDtos = communities.Adapt<IEnumerable<CommunityUsingBaseDto>>();
        var hasMore = totalCount > (request.Skip + request.Take);

        return new PagedResponseDto<CommunityUsingBaseDto>
        {
            Items = communityDtos,
            TotalCount = totalCount,
            HasMore = hasMore
        };
    }
} 