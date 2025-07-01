using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Posts.Queries;

public record GetPostsByCommunityIdQuery(int CommunityId, int Skip, int Take) : IRequest<PagedResponseDto<BasePostDto>>;

public class GetPostsByCommunityIdQueryHandler : IRequestHandler<GetPostsByCommunityIdQuery, PagedResponseDto<BasePostDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPostsByCommunityIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponseDto<BasePostDto>> Handle(GetPostsByCommunityIdQuery request, CancellationToken cancellationToken)
    {
        var communitiesId = new List<int> { request.CommunityId };
        var posts = await _unitOfWork.Posts.GetForCommunityIds(communitiesId, request.Skip, request.Take);
        var totalPosts = await _unitOfWork.Posts.CountPostsAsync(communitiesId);
        var hasMore = totalPosts > (request.Skip + request.Take);

        return new PagedResponseDto<BasePostDto>
        {
            Items = posts.Adapt<IEnumerable<BasePostDto>>(),
            HasMore = hasMore,
            TotalCount = totalPosts
        };
    }
} 