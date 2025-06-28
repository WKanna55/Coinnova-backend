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

    public IQueryable<Community> GetQueryCommunitiesByCategoryId(int categoryId)
    {
        return _context.CommunityCategory
            .AsNoTracking()
            .Where(cc => cc.IdCategory == categoryId)
            .Select(cc => new Community
            {
                Id = cc.IdCommunityNavigation.Id,
                Name = cc.IdCommunityNavigation.Name,
                Description = cc.IdCommunityNavigation.Description,
                Imageurl = cc.IdCommunityNavigation.Imageurl,
                Createdat = cc.IdCommunityNavigation.Createdat,
                MemberCount = cc.IdCommunityNavigation.CommunityMember.Count()
            });
    }

    public IQueryable<Community> GetQueryCommunities()
    {
        return _context.Community
            .AsNoTracking()
            .Select(c => new Community
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Imageurl = c.Imageurl,
                Createdat = c.Createdat,
                MemberCount = c.CommunityMember.Count()
            });
    }

    public async Task<List<Community>> GetForInstitutions(int institutionId)
    {
        var communities = await _context.Community.Where(c => c.IdInstitution == institutionId).ToListAsync();
        return communities;
    }

    public async Task<IList<int>> GetIdsForSuscribedUserGeneral(int userId)
    {
        var communityIds = await _context.CommunityMember
            .Where(cm => cm.IdUser == userId)
            .Select(cm => cm.IdCommunity)
            .ToListAsync();
        return communityIds;
    }
    
    public async Task<IList<int>> GetIdsForSuscribedUserInstitution( int userId)
    {
        var communityIds = await _context.CommunityMember
            .Where(cm => cm.IdUser == userId && cm.IdCommunityNavigation.IdInstitution != null)
            .Select(cm => cm.IdCommunity)
            .ToListAsync();
        return communityIds;
    }

    public async Task<IEnumerable<Community>> SearchCommunitiesByName(string name)
    {
        return await _context.Community
            .Where(c => c.Name.ToLower().Contains(name.ToLower()))
            .Select(c => new Community
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Imageurl = c.Imageurl,
                Createdat = c.Createdat,
                MemberCount = c.CommunityMember.Count()
            })
            .ToListAsync();
    }
}