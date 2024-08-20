namespace Opcion1LosBorbotones.Domain.Services;

public interface ICrudOperations<T> where T : Entity
{
    Task<T> CreateAsync(T entity);
    Task<T?> ReadAsync(string id);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(string id);
}

