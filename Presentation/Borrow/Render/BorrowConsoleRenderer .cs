using Opcion1LosBorbotones.Domain.Entity;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BorrowConsoleRenderer : IBorrowConsoleRenderer
{
    public long GetMembershipNumber()
    {
        long patronId = AnsiConsole.Ask<long>("What patron Membership Number do you want to register?: ");
        return patronId;
    }

    public long GetISBN()
    {
        long bookId = AnsiConsole.Ask<long>("What book ISBN do you want to borrow?: ");
        return bookId;
    }

    public void DisplayBorrowDetails(Borrow borrow)
    {
        AnsiConsole.MarkupLine("[bold green]Review the BORROW details before confirming:[/]");
        AnsiConsole.MarkupLine($"[bold] Borrow date [/]: {borrow.BorrowDate}");
        AnsiConsole.MarkupLine($"[bold] Due date [/]: {borrow.DueDate}");
    }

    public bool ConfirmBorrow()
    {
        return AnsiConsole.Confirm("[bold] Do you want to register this borrow? [/]");
    }
}