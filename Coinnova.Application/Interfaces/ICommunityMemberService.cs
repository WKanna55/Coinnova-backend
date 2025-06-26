namespace Coinnova.Application.Interfaces;

public interface ICommunityMemberService
{
    Task<bool> SubscribeUserToCommunity(int userId, int communityId);
}