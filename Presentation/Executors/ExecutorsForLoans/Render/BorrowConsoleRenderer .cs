using Opcion1LosBorbotones.Domain.Entity;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BorrowConsoleRenderer : IBorrowConsoleRenderer
{
    public Guid GetPatronId()
    {
        string patronId = AnsiConsole.Ask<string>("What patron id do you want to register?: ");
        return new Guid(patronId);
    }

    public Guid GetBookId()
    {
        string bookId = AnsiConsole.Ask<string>("What book id do you want to borrow?: ");
        return new Guid(bookId);
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