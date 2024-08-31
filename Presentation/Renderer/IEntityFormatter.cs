using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation.Renderer;

public abstract class IEntityFormatter<T> where T : IEntity
{
    protected T _entity;

    protected IEntityFormatter(T entity)
    {
        _entity = entity;
    }

    public T Entity
    {
        get => _entity;
    }
}