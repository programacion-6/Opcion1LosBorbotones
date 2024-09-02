using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Domain.Validator.Exceptions;
using Opcion1LosBorbotones.Infrastructure.Services.Borrows;
using Opcion1LosBorbotones.Logger.LogManagement;
using Opcion1LosBorbotones.Presentation.Renderer.BorrowFormatter;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Executors;

public class LoanHandlerExecutor : IExecutor
{
    private readonly IBorrowService _borrowService;
    private readonly IBorrowConsoleRenderer _borrowConsoleRenderer;
    private readonly IPatronRepository _patronRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowRepository _borrowRepository;

    public LoanHandlerExecutor(IBorrowService borrowService,
                         IPatronRepository patronRepository,
                         IBookRepository bookRepository,
                         IBorrowRepository borrowRepository)
    {
        _borrowService = borrowService;
        _borrowConsoleRenderer = new BorrowConsoleRenderer();
        _patronRepository = patronRepository;
        _bookRepository = bookRepository;
        _borrowRepository = borrowRepository;
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
                        "2. Request a Return",
                        "3. Go back"
                    ])
            );

            switch (option)
            {
                case "1. Request a borrow":
                    await RegisterNewBorrow();
                    break;
                case "2. Request a Return":
                    await ReturnBook();
                    break;
                case "3. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private async Task RegisterNewBorrow()
    {
        AppPartialsRenderer.RenderHeader();
        ConsoleMessageRenderer.RenderIndicatorMessage("New loan");

        var selectedBook = await SelectionHelper<Book>.SelectItemAsync(
                                _bookRepository,
                                "Select the book you want to borrow:",
                                "No books available for borrowing.",
                                book => $"{book.Title} | {book.Author} | ISBN: {book.Isbn}"
                            );

        var selectedPatron = await SelectionHelper<Patron>.SelectItemAsync(
                                _patronRepository,
                                "Select the patron who is borrowing the book:",
                                "No patrons available for borrowing.",
                                patron => $"{patron.Name} | {patron.ContactDetails} | {patron.MembershipNumber}"
                            );

        if (selectedBook == null || selectedPatron == null)
        {
            ConsoleMessageRenderer.RenderErrorMessage("Book or Patron selection was cancelled or invalid.");
            AppPartialsRenderer.RenderConfirmationToContinue();
            return;
        }

        if (_borrowConsoleRenderer.ConfirmBorrow())
        {
            try
            {
                var borrow = await _borrowService.RegisterNewBorrow(selectedPatron.Id, selectedBook.Id);
                ConsoleMessageRenderer.RenderSuccessMessage($"New borrow registered:\n");
                ResultRenderer.RenderResult(borrow,
                                b => new DetailedBorrowFormatter(b,
                                                                _bookRepository,
                                                                _patronRepository).ToString()
                                                                );
                _borrowConsoleRenderer.DisplayBorrowDetails(borrow);
            }
            catch (InvalidOperationException ex)
            {
                ErrorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, ex.Message, ex);
                AnsiConsole.MarkupLine($"[bold italic red]{ex.Message}[/]");
            }
        }
        else
        {
            ConsoleMessageRenderer.RenderInfoMessage("No borrow registered");
        }

        AppPartialsRenderer.RenderConfirmationToContinue();
    }

    private async Task ReturnBook()
    {
        AppPartialsRenderer.RenderHeader();
        ConsoleMessageRenderer.RenderIndicatorMessage("Return Book");

        var selectedBorrow = await SelectionHelper<Borrow>.SelectBorrowItemAsync(
                                _borrowRepository,
                                "Select the borrow you want to return:",
                                "No borrowed books available for return.",
                                b => new DetailedBorrowFormatter(b, _bookRepository, _patronRepository).ToString()
                            );

        if (selectedBorrow == null)
        {
            ConsoleMessageRenderer.RenderErrorMessage("Borrow selection was cancelled or invalid.");
            AppPartialsRenderer.RenderConfirmationToContinue();
            return;
        }

        if (_borrowConsoleRenderer.ConfirmReturn())
        {
            try
            {
                bool success = await _borrowRepository.UpdateBorrowStatus(selectedBorrow.Id, BorrowStatus.Returned);
                if (success)
                {
                    ConsoleMessageRenderer.RenderSuccessMessage("Book returned successfully.");
                }
                else
                {
                    ConsoleMessageRenderer.RenderErrorMessage("Failed to return the book.");
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, ex.Message, ex);
                ConsoleMessageRenderer.RenderErrorMessage($"Error: {ex.Message}");
            }
        }
        else
        {
            ConsoleMessageRenderer.RenderInfoMessage("No borrow returned");
        }

        AppPartialsRenderer.RenderConfirmationToContinue();
    }
}