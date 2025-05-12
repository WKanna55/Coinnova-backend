using System.Collections;
using Coinnova.Application.Interfaces;
using Coinnova.Domain.Interfaces;
using Coinnova.Domain.Interfaces.Base;
using Coinnova.Infrastructure.Context;

namespace Coinnova.Infrastructure.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable? _repositories;
    private readonly ApplicationDbContext _context;
    
    // inyeccion repositorios especificos
    public IUserRepository Users { get; }
    public IPostRepository Posts { get; }
    public ICommunityRepository Communities { get; }
    public ICommentRepository Comments { get; }
    public ICategoryRepository Categories { get; }
    
    public UnitOfWork(ApplicationDbContext context, IUserRepository usersRepository,
        IPostRepository postRepository, ICommunityRepository communityRepository, ICommentRepository commentsRepository,
        ICategoryRepository categoryRepository)
    {
        _context = context;
        _repositories = new Hashtable();
        // inyeccion repositorio especificos
        Users = usersRepository;
        Categories = categoryRepository;
        Posts = postRepository;
        Communities = communityRepository;
        Comments = commentsRepository;
    }

    public Task<int> Complete()
    {
        return _context.SaveChangesAsync();
    }

    /*
     * Funcion para usar un repositorio generico con cualquier entidad
     */
    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);

        if (_repositories.ContainsKey(type))
        {
            return (IRepository<TEntity>)_repositories[type];
        }

        var repositoryType = typeof(Repository<>);
        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

        if (repositoryInstance != null)
        {
            _repositories.Add(type, repositoryInstance);
            return (IRepository<TEntity>)repositoryInstance;
        }
        
        throw new Exception($"No se pudo crear el repositorio para este tipo {type}");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    
}