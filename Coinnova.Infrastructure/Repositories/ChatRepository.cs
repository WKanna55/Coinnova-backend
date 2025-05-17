using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;

namespace Coinnova.Infrastructure.Repositories;

public class ChatRepository: Repository<Chat>, IChatRepository
{
    private readonly ApplicationDbContext _context;

    public ChatRepository(ApplicationDbContext context) : base(context)
    {
        this._context = context;
    }
}