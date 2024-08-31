using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Presentation.Renderer;

namespace Opcion1LosBorbotones.Presentation;

public interface IEntityFormatterFactory<T> where T : IEntity
{
    public Task<IEntityFormatter<T>?> CreateDetailedFormatter(T? entity);
}
