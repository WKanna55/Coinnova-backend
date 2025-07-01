using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Posts.Queries;

public record GetUserFeedQuery(int UserId, int Skip, int Take) : IRequest<PagedResponseDto<BasePostDto>>;

public class GetUserFeedQueryHandler : IRequestHandler<GetUserFeedQuery, PagedResponseDto<BasePostDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserFeedQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponseDto<BasePostDto>> Handle(GetUserFeedQuery request, CancellationToken cancellationToken)
    {
        var subscribedUserCommunitiesIds = await _unitOfWork.Communities.GetIdsForSuscribedUserGeneral(request.UserId);
        var posts = await _unitOfWork.Posts.GetForCommunityIds(subscribedUserCommunitiesIds, request.Skip, request.Take);
        var totalPosts = await _unitOfWork.Posts.CountPostsAsync(subscribedUserCommunitiesIds);
        var hasMore = totalPosts > (request.Skip + request.Take);

        return new PagedResponseDto<BasePostDto>
        {
            Items = posts.Adapt<IEnumerable<BasePostDto>>(),
            HasMore = hasMore,
            TotalCount = totalPosts
        };
    }
} 