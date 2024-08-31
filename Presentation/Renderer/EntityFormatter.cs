using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation.Renderer;

public abstract class EntityFormatter<T> where T : IEntity
{
    protected T _entity;

    protected EntityFormatter(T entity)
    {
        _entity = entity;
    }

    public T Entity
    {
        get => _entity;
    }
}