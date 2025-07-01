using Coinnova.Application.Dtos.Common;
using Coinnova.Application.Dtos.Post;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Posts.Queries;

public record GetInstitutionUserFeedQuery(int UserId, int Skip, int Take) : IRequest<PagedResponseDto<BasePostDto>>;

public class GetInstitutionUserFeedQueryHandler : IRequestHandler<GetInstitutionUserFeedQuery, PagedResponseDto<BasePostDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInstitutionUserFeedQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponseDto<BasePostDto>> Handle(GetInstitutionUserFeedQuery request, CancellationToken cancellationToken)
    {
        var subscribedUserInstitutionCommunitiesIds = await _unitOfWork.Communities.GetIdsForSuscribedUserInstitution(request.UserId);
        var posts = await _unitOfWork.Posts.GetForCommunityIds(subscribedUserInstitutionCommunitiesIds, request.Skip, request.Take);
        var totalPosts = await _unitOfWork.Posts.CountPostsAsync(subscribedUserInstitutionCommunitiesIds);
        var hasMore = totalPosts > (request.Skip + request.Take);

        return new PagedResponseDto<BasePostDto>
        {
            Items = posts.Adapt<IEnumerable<BasePostDto>>(),
            HasMore = hasMore,
            TotalCount = totalPosts
        };
    }
} 