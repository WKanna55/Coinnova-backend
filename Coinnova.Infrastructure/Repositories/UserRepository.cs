using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Coinnova.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext _context) : base(_context)
    {
        this._context = _context;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.User
            .Include(u => u.IdRoleNavigation)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public override async Task<User?> GetById(int id)
    {
        return await _context.User
            .Include(u => u.IdRoleNavigation)
            .Include(u => u.IdInstitutionNavigation)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
}