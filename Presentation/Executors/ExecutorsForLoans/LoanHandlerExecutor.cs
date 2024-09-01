using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Borrows;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Executors;

public class LoanHandlerExecutor : IExecutor
{
    private readonly IBorrowService _borrowService;
    private readonly IBorrowConsoleRenderer _borrowConsoleRenderer;
    private IEntityFormatterFactory<Borrow> _formatterFactoryBorrow;
    private readonly IPatronRepository _patronRepository;
    private readonly IBookRepository _bookRepository;

    public LoanHandlerExecutor(IBorrowService borrowService,
                         IEntityFormatterFactory<Borrow> entityFormatterFactoryBorrow,
                         IPatronRepository patronRepository,
                         IBookRepository bookRepository)
    {
        _borrowService = borrowService;
        _borrowConsoleRenderer = new BorrowConsoleRenderer();
        _formatterFactoryBorrow = entityFormatterFactoryBorrow;
        _patronRepository = patronRepository;
        _bookRepository = bookRepository;
    }

    public async Task Execute()
    {
        bool goBack = false;
        while (!goBack)
        {
            AppPartialsRenderer.RenderHeader();
            ConsoleMessageRenderer.RenderIndicatorMessage("Borrow Menu");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices(
                    [
                        "1. Request a borrow",
                        "2. Go back"
                    ])
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
            AppPartialsRenderer.RenderHeader();
            ConsoleMessageRenderer.RenderIndicatorMessage("New loan");
            var books = (await _bookRepository.GetAll()).ToArray();

            if (books == null || books.Length == 0)
            {
                AnsiConsole.MarkupLine("[red]No books available for borrowing.[/]");
                return;
            }

            var selectedBook = AnsiConsole.Prompt(
                new SelectionPrompt<Book>()
                    .Title("Select the book you want to borrow:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Scroll up and down to see more options)[/]")
                    .AddChoices(books)
                    .UseConverter(book => $"{book.Title} | {book.Author} | ISBN: {book.Isbn}")
            );

            var bookId = selectedBook.Id;


            var patrons = (await _patronRepository.GetAll()).ToArray();

            if (patrons == null || patrons.Length == 0)
            {
                AnsiConsole.MarkupLine("[red]No patrons available for borrowing.[/]");
                return;
            }

            var selectedPatron = AnsiConsole.Prompt(
                new SelectionPrompt<Patron>()
                    .Title("Select the patron who is borrowing the book:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Scroll up and down to see more options)[/]")
                    .AddChoices(patrons)
                    .UseConverter(patron => $"{patron.Name} | {patron.ContactDetails} | Membership Number: {patron.MembershipNumber}")
            );

            var patronId = selectedPatron.Id;
            // var patronUUID = _borrowConsoleRenderer.GetPatronId();
            // var bookUUID = _borrowConsoleRenderer.GetBookId();
            //var borrow = await _borrowService.RegisterNewBorrow(patronUUID, bookUUID);
            //_borrowConsoleRenderer.DisplayBorrowDetails(borrow);

            if (_borrowConsoleRenderer.ConfirmBorrow())
            {
                try
                {
                    var borrow = await _borrowService.RegisterNewBorrow(patronId, bookId);


                    //_formatterFactoryBorrow.CreateDetailedFormatter(borrow);


                    //AnsiConsole.MarkupLine($"[bold italic green]New borrow registered:[/]\n{formatter}");
                    ConsoleMessageRenderer.RenderSuccessMessage($"New borrow registered:\n");
                    _formatterFactoryBorrow.CreateDetailedFormatter(borrow);
                    _borrowConsoleRenderer.DisplayBorrowDetails(borrow);
                }
                catch (InvalidOperationException ex)
                {
                    AnsiConsole.MarkupLine($"[bold italic red]{ex.Message}[/]");
                }
                //ConsoleMessageRenderer.RenderSuccessMessage($"New borrow registered {borrow}");
            }
            else
            {
                ConsoleMessageRenderer.RenderInfoMessage("No borrow registered");
            }

            AppPartialsRenderer.RenderConfirmationToContinue();
        }
        catch (Exception)
        {
            ConsoleMessageRenderer.RenderErrorMessage("The data entered is not correct, please enter correct data");
            AppPartialsRenderer.RenderConfirmationToContinue();
        }
    }
}