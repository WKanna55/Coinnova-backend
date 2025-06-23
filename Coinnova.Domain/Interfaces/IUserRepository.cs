using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email);
    Task<IEnumerable<User>> GetFirstMembersByCommunityId(int communityId, int count);
    Task<User?> GetWithRoleByEmail(string email);
    Task<User?> GetByIdWithRelations(int id);
}