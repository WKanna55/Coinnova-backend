using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;
using MediatR;

namespace Coinnova.Application.UseCases.CommunityMembers.Commands;

public sealed record SubscribeUserToCommunityCommand(int UserId, int CommunityId) : IRequest<bool>;

internal sealed record SubscribeUserToCommunityCommandHandler(IUnitOfWork UnitOfWork) 
    : IRequestHandler<SubscribeUserToCommunityCommand, bool>
{
    public async Task<bool> Handle(SubscribeUserToCommunityCommand request, CancellationToken cancellationToken)
    {
        var communityMember = new CommunityMember
        {
            IdUser = request.UserId,
            IdCommunity = request.CommunityId
        };
        await UnitOfWork.Repository<CommunityMember>().Add(communityMember);
        var result = await UnitOfWork.Complete();
        return result > 0;
    }
}