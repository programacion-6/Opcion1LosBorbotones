using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain.Repository;

public interface IRepository<T> where T : IEntity
{
    Task<bool> Save(T entity);
    Task<bool> Update(T entity);
    Task<bool> Delete(long id);
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll();
}
