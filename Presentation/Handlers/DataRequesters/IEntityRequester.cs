using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones;

public interface IEntityRequester<T> where T : IEntity
{
    public T AskForEntity();
}