using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation.Renderer.PatronFormatter;

public class DetailedPatronFormatter : EntityFormatter<Patron>
{
    public DetailedPatronFormatter(Patron entity) : base(entity)
    {
    }

    public override string ToString()
    {
        return $"[bold plum3]----- |{_entity.Name}| -----[/]\n" +
               $"[bold plum3]MembershipNumber:[/] {_entity.MembershipNumber}\n" +
               $"[bold plum3]ContactDetails:[/] {_entity.ContactDetails}\n";
    }
}