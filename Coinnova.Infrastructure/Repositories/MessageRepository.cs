using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces;
using Coinnova.Infrastructure.Context;
using Coinnova.Infrastructure.Repositories.Base;

namespace Coinnova.Infrastructure.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context) : base(context)
    {
        this._context = context;
    }
}