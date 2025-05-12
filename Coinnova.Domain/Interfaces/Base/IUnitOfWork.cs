namespace Coinnova.Domain.Interfaces.Base;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> Complete();
    
    // inyeccion de repositorios especificos
    IUserRepository Users { get; }
    IPostRepository Posts { get; }
    ICommunityRepository Communities { get; }
    ICommentRepository Comments { get; }
}