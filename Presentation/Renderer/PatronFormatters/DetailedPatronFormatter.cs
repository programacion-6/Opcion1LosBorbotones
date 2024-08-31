using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation.Renderer.PatronFormatter;

public class DetailedPatronFormatter : IEntityFormatter<Patron>
{
    public DetailedPatronFormatter(Patron entity) : base(entity)
    {
    }

    public override string ToString()
    {
        return $"Name: {_entity.Name}, MembershipNumber: {_entity.MembershipNumber}, ContactDetails: {_entity.ContactDetails}";
    }
}