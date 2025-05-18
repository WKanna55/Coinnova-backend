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
    
    public IQueryable<Community> QueryComunityWithMembers()
    {
        var communities = _context.Community
            .Include(c => c.CommunityMember);

        return communities;
    }

    public Task<IQueryable<object>> GetQueryCommunitiesByCategoryId(int id)
    {
        var query =  _context.CommunityCategory
            .Where(cc => cc.IdCategory == id)
            .Select(cc => cc.IdCommunity)
            .Distinct()
            .Join(
                _context.Community,
                communityId => communityId,
                c => c.Id,
                (communityId, c) => new { Community = c }
            )
            .GroupJoin(
                _context.CommunityMember,
                c => c.Community.Id,
                cm => cm.IdCommunity,
                (c, members) => new 
                {
                    Id = c.Community.Id,
                    Name = c.Community.Name,
                    Description = c.Community.Description,
                    ImageUrl = c.Community.Imageurl,
                    CreatedAt = c.Community.Createdat,
                    Members = members.Count()
                });

        return Task.FromResult<IQueryable<object>>(query);
    }

    public Task<IQueryable<object>> GetQueryCommunitiesWithMembers()
    {
        var query = _context.Community
            .GroupJoin(
                _context.CommunityMember,
                c => c.Id,
                cm => cm.IdCommunity,
                (c, members) => new 
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.Imageurl,
                    CreatedAt = c.Createdat,
                    Members = members.Count()
                });

        return Task.FromResult<IQueryable<object>>(query);
    }
}