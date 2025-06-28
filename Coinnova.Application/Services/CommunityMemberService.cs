using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Application.Services;

public class CommunityMemberService : ICommunityMemberService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunityMemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> SubscribeUserToCommunity(int userId, int communityId)
    {
        var communityMember = new CommunityMember
        {
            IdUser = userId,
            IdCommunity = communityId
        };

        await _unitOfWork.Repository<CommunityMember>().Add(communityMember);
        var result = await _unitOfWork.Complete();

        return result > 0;
    }
}