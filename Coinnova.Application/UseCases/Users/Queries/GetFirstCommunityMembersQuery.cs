using Coinnova.Application.Dtos.User;
using Coinnova.Domain.Interfaces.Base;
using Mapster;
using MediatR;

namespace Coinnova.Application.UseCases.Users.Queries;

public class GetFirstCommunityMembersQuery : IRequest<IEnumerable<UserSimpleDto>>
{
    public int CommunityId { get; set; }
}

public class GetFirstCommunityMembersQueryHandler : IRequestHandler<GetFirstCommunityMembersQuery, IEnumerable<UserSimpleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFirstCommunityMembersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserSimpleDto>> Handle(GetFirstCommunityMembersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetFirstMembersByCommunityId(request.CommunityId, 6);
        return users.Adapt<IEnumerable<UserSimpleDto>>();
    }
} 