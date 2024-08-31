using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation.Renderer;

public abstract class IEntityDetailedFormatter<T> : IEntityFormatter<T> where T : IEntity
{
    protected IEntityDetailedFormatter(T entity) : base(entity)
    {
    }

    public abstract Task BorrowRelatedData();
}