using Opcion1LosBorbotones.Domain.Entity;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BorrowConsoleRenderer : IBorrowConsoleRenderer
{
    public void DisplayBorrowDetails(Borrow borrow)
    {
        AnsiConsole.MarkupLine("[bold green]Book return dates:[/]");
        AnsiConsole.MarkupLine($"[bold] Borrow date [/]: {borrow.BorrowDate}");
        AnsiConsole.MarkupLine($"[bold] Due date [/]: {borrow.DueDate}");
    }

    public bool ConfirmBorrow()
    {
        return AnsiConsole.Confirm("[bold] Do you want to register this borrow? [/]");
    }

    public bool ConfirmReturn()
    {
        return AnsiConsole.Confirm("[bold] Do you want to return this book? [/]");
    }
}