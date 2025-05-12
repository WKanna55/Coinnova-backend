using Coinnova.Application.Interfaces;
using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;

namespace Coinnova.Infrastructure.Repositories;

public class CommunityRepository : Repository<Community>, ICommunityRepository
{
    private readonly ApplicationDbContext _context;
    
    public CommunityRepository(ApplicationDbContext _context) : base(_context)
    {
        this._context = _context;
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