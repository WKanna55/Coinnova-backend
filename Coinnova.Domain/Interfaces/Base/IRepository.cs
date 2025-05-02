namespace Coinnova.Domain.Interfaces.Base;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(int id);
    Task Add(T entity);
    Task Update(T entity);
    Task<bool> Delete(int id);
}