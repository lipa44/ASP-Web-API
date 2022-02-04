namespace DataAccess.Repositories;

public interface IRepository<T>
{
    Task<IReadOnlyCollection<T>> GetAll();
    Task<T> FindItem(Guid id);
    Task<T> GetItem(Guid id);
    Task<T> Create(T item);
    Task<T> Update(T item);
    Task<T> Delete(Guid id);
}