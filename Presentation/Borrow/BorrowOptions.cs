using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Services.Borrows;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BorrowOptions
{
    private readonly IBorrowService _borrowService;
    private readonly IBorrowConsoleRenderer _borrowConsoleRenderer;
    private IEntityFormatterFactory<Borrow> _formatterFactoryBorrow;

    public BorrowOptions(IBorrowService borrowService, IEntityFormatterFactory<Borrow> entityFormatterFactoryBorrow)
    {
        _borrowService = borrowService;
        _borrowConsoleRenderer = new BorrowConsoleRenderer();
        _formatterFactoryBorrow = entityFormatterFactoryBorrow;
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
        try
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Register a new borrow[/]");

            var isbn = _borrowConsoleRenderer.GetISBN();
            var membershipNumber = _borrowConsoleRenderer.GetMembershipNumber();

            var bookId = await _borrowService.GetBookIdByISBN(isbn);
            var patronId = await _borrowService.GetPatronIdByMembershipNumber(membershipNumber);

            if (bookId == Guid.Empty || patronId == Guid.Empty)
            {
                AnsiConsole.MarkupLine("[bold red]Invalid ISBN or Membership Number[/]");
                AnsiConsole.Markup("[blue] Press Enter to go back to the Borrow Menu.[/]");
                Console.ReadLine();
                return;
            }

            if (_borrowConsoleRenderer.ConfirmBorrow())
            {
                var borrow = await _borrowService.RegisterNewBorrow(patronId, bookId);

                var formatter = await _formatterFactoryBorrow.CreateDetailedFormatter(borrow);

                AnsiConsole.MarkupLine($"[bold italic green]New borrow registered:[/] {formatter}");
                _borrowConsoleRenderer.DisplayBorrowDetails(borrow);
            }
            else
            {
                AnsiConsole.MarkupLine("[bold green]No borrow registered[/]");
            }

            AnsiConsole.Markup("[blue] Press Enter to go back to the Patron Menu.[/]");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red] The data entered is not correct, please enter correct data.[/]\n");
            AnsiConsole.Markup("[blue] Press Enter to go back to the Borrow Menu.[/]");
            Console.ReadLine();
        }
    }
}