namespace Opcion1LosBorbotones.Domain;

public abstract class Entity
{
    public string Id { get; }

    protected Entity(string id)
    {
        Id = id;
    }
}
