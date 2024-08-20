using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain.Services;

public interface ICrudOperations<T> where T : IEntity
{
    Task<T> CreateAsync(T entity);
    Task<T?> ReadAsync(string id);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(string id);
}

