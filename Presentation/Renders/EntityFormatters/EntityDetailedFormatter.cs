using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation.Renderer;

public abstract class EntityDetailedFormatter<T> : EntityFormatter<T> where T : IEntity
{
    protected EntityDetailedFormatter(T entity) : base(entity)
    {
    }

    public abstract Task BorrowRelatedData();
}