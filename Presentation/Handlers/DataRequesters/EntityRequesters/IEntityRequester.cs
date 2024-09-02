using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation.Handlers;

public interface IEntityRequester<T> where T : IEntity
{
    public T AskForEntity();
}