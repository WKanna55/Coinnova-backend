using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Repositories;

public class CommunityRepository : Repository<Community>, ICommunityRepository
{
    private readonly ApplicationDbContext _context;
    
    public CommunityRepository(ApplicationDbContext context) : base(context)
    {
        this._context = context;
    }

    public async Task<int> CountCommunityMembersByCommunityId(int id)
    {
        var number = await _context.CommunityMember.Where(cm => cm.IdCommunity == id).CountAsync();
        return number;
    }
    
    public IQueryable<object> GetPopularCommunities()
    {
        var communities = _context.Community
            .Select(c => new
            {
                Community = c,
                MemberCount = _context.CommunityMember.Count(cm => cm.IdCommunity == c.Id)
            })
            .OrderByDescending(c => c.MemberCount);

        return communities;
    }
}