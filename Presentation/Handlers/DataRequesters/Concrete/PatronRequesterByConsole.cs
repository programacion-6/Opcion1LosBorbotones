using Opcion1LosBorbotones;
using Opcion1LosBorbotones.Domain.Entity;
using Spectre.Console;

namespace LibrarySystem;

public class PatronRequesterByConsole : IEntityRequester<Patron>
{
    private const Patron? _unrequestedPatron = null;

    public Patron AskForEntity()
    {
        Patron? requestedPatron = _unrequestedPatron;

        while (requestedPatron is _unrequestedPatron)
        {
            requestedPatron = ReceivePatronByConsole();
            requestedPatron = ConfirmPatronReceived(requestedPatron);
        }

        return requestedPatron;
    }

    private Patron ReceivePatronByConsole()
    {
        var id = Guid.NewGuid();
        var name = AnsiConsole.Ask<string>("Enter the patron name: ");
        var membershipNumber = AnsiConsole.Ask<long>("Enter the membership number: ");
        var contactDetailNumber = AnsiConsole.Ask<long>("Enter the contact detail number: ");
        var patron = new Patron(id, name, membershipNumber, contactDetailNumber);
        return patron;
    }

    private Patron? ConfirmPatronReceived(Patron bookReceived)
    {
        RenderBookReceived(bookReceived);
        var wasConfirmed = AnsiConsole.Confirm("[bold] Do you want to continue? [/]");
        if (!wasConfirmed)
        {
            AnsiConsole.MarkupLine("[bold italic cyan]Insert the data again[/]");
            return _unrequestedPatron;
        }

        return bookReceived;
    }

    private void RenderBookReceived(Patron patronReceived)
    {
        AnsiConsole.MarkupLine("[bold green]Review the Patron details before confirming:[/]");
        AnsiConsole.MarkupLine($"[bold] Name [/]: {patronReceived.Name}");
        AnsiConsole.MarkupLine($"[bold] Membership number [/]: {patronReceived.MembershipNumber}");
        AnsiConsole.MarkupLine($"[bold] Contact details [/]: {patronReceived.ContactDetails}");
    }

}
