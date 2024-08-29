using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BorrowOptions
{
    private readonly IBorrowRepository _borrowRepository;

    public BorrowOptions(IBorrowRepository borrowRepository)
    {
        _borrowRepository = borrowRepository;
    }

    public async Task BorrowInitialOptions()
    {
        bool goBack = false;
        while (!goBack)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Borrow Menu[/]");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "1. Request a borrow",
                        "2. Go back"
                    })
            );

            switch (option)
            {
                case "1. Request a borrow":
                    await RegisterNewBorrow();
                    break;
                case "2. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private async Task RegisterNewBorrow()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Register a new borrow[/]");
        
        Guid borrowUUID = Guid.NewGuid();
        string patronId = AnsiConsole.Ask<string>("What patron id do you want to register?: ");
        Guid patronUUID = new Guid(patronId);
        string bookId = AnsiConsole.Ask<string>("What book id do you want to borrow?: ");
        Guid bookUUID = new Guid(bookId);
        BorrowStatus borrowStatus = BorrowStatus.Borrowed;
        DateTime borrowDate = DateTime.Today;
        DateTime dueDate = DateTime.Today.AddDays(15);
        
        AnsiConsole.MarkupLine("[bold green]Review the BORROW details before confirming:[/]");
        AnsiConsole.MarkupLine($"[bold] Borrow date [/]: {borrowDate}");
        AnsiConsole.MarkupLine($"[bold] Due date [/]: {dueDate}");
        
        var confirm = AnsiConsole.Confirm("[bold] Do you want to register this borrow? [/]");

        if (confirm)
        {
            Borrow newBorrow = new Borrow(borrowUUID, patronUUID, bookUUID, borrowStatus, dueDate, borrowDate);
            await _borrowRepository.Save(newBorrow);
            AnsiConsole.MarkupLine($"[bold italic green]New borrow registered:[/] {newBorrow}");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold green]No borrow registered");
        }
        
        AnsiConsole.Markup("[blue] Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }
}