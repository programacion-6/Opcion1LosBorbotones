using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Domain.Validator;
using Opcion1LosBorbotones.Domain.Validator.Exceptions;
using Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;
using Opcion1LosBorbotones.Logger.LogManagement;
using Opcion1LosBorbotones.Presentation.Handlers;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Executors;

public class BookHandlerExecutor : IExecutor
{
    private readonly IBookRepository _bookRepository;
    private readonly IEntityRequester<Book> _bookRequester;
    private readonly BookValidator _bookValidator;
    private readonly BookFinderExecutor _bookFinder;

    public BookHandlerExecutor(IBookRepository bookRepository, IEntityRequester<Book> bookRequester, BookFinderExecutor bookFinder)
    {
        _bookRepository = bookRepository;
        _bookRequester = bookRequester;
        _bookFinder = bookFinder;
        _bookValidator = new();
    }

    public async Task Execute()
    {
        bool goBack = false;
        while (!goBack)
        {
            AppPartialsRenderer.RenderHeader();
            ConsoleMessageRenderer.RenderIndicatorMessage("Book Menu");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices([
                        "1. Register a new book",
                        "2. Delete a book",
                        "3. Edit a book",
                        "4. Search books",
                        "5. Go back"
                    ])
            );

            switch (option)
            {
                case "1. Register a new book":
                    await RegisterNewBook();
                    break;
                case "2. Delete a book":
                    await DeleteBook();
                    break;
                case "3. Edit a book":
                    await EditBook();
                    break;
                case "4. Search books":
                    await _bookFinder.Execute();
                    break;
                case "5. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private async Task RegisterNewBook()
    {
        AppPartialsRenderer.RenderHeader();
        ConsoleMessageRenderer.RenderIndicatorMessage("Register a new book");

        try
        {
            var newBook = _bookRequester.AskForEntity();
            _bookValidator.ValidateBook(newBook);
            await _bookRepository.Save(newBook);
            ConsoleMessageRenderer.RenderSuccessMessage("New book registered");
        }
        catch (BookException bookException)
        {
            ErrorLogger.LogErrorBasedOnSeverity(bookException.Severity, bookException.Message, bookException);
            ConsoleMessageRenderer.RenderErrorMessage(bookException.Message);
            ConsoleMessageRenderer.RenderErrorMessage(bookException.ResolutionSuggestion);
        }
        catch (Exception exception)
        {
            ErrorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, exception.Message, exception);
            ConsoleMessageRenderer.RenderErrorMessage(exception.Message);
        }

        AppPartialsRenderer.RenderConfirmationToContinue();
    }

    private async Task DeleteBook()
    {
        AppPartialsRenderer.RenderHeader();
        ConsoleMessageRenderer.RenderIndicatorMessage("Delete a book");

        var bookToDelete = await SelectionHelper<Book>.SelectItemAsync(
                                    _bookRepository,
                                    "Select the book you want to delete:",
                                    "There are no books available for deletion.",
                                    book => $"{book.Title} | {book.Author} | ISBN: {book.Isbn}"
                                );
        
        if (bookToDelete == null)
        {
            ConsoleMessageRenderer.RenderErrorMessage("No book selected for deletion.");
            AppPartialsRenderer.RenderConfirmationToContinue();
            return;
        }

        var wasConfirmed = AnsiConsole.Confirm($"Are you sure you want to delete this book? [yellow]{bookToDelete.Title}[/]?");

        if (wasConfirmed)
        {
            try
            {
                await _bookRepository.Delete(bookToDelete.Isbn);
                ConsoleMessageRenderer.RenderSuccessMessage("The book has been successfully deleted.");
            }
            catch (BookException bookException)
            {
                ErrorLogger.LogErrorBasedOnSeverity(bookException.Severity, bookException.Message, bookException);
                ConsoleMessageRenderer.RenderErrorMessage(bookException.Message);
                ConsoleMessageRenderer.RenderErrorMessage(bookException.ResolutionSuggestion);
            }
            catch (Exception exception)
            {
                ErrorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, exception.Message, exception);
                ConsoleMessageRenderer.RenderErrorMessage(exception.Message);
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic]Canceled.[/]");
        }

        AppPartialsRenderer.RenderConfirmationToContinue();
    }

    private async Task EditBook()
    {
        AppPartialsRenderer.RenderHeader();
        ConsoleMessageRenderer.RenderIndicatorMessage("Edit book");

        try
        {
            var bookToEdit = await SelectionHelper<Book>.SelectItemAsync(
                                    _bookRepository,
                                    "Select the book you want to edit:",
                                    "There are no books available for editing.",
                                    book => $"{book.Title} | {book.Author} | ISBN: {book.Isbn}"
                                );
            
            if (bookToEdit == null)
            {
                ConsoleMessageRenderer.RenderErrorMessage("No book selected for editing.");
                AppPartialsRenderer.RenderConfirmationToContinue();
                return;
            }
            var wasConfirmed = AnsiConsole.Confirm($"Are you sure you want to edit this book? [yellow]{bookToEdit.Title}[/]?");

            if (wasConfirmed)
            {
                try
                {
                    var editedBook = _bookRequester.AskForEntity();
                    editedBook.Id = bookToEdit.Id;
                    _bookValidator.ValidateBook(editedBook);
                    await _bookRepository.Update(editedBook);
                    ConsoleMessageRenderer.RenderSuccessMessage("Book edited");
                }
                catch (BookException bookException)
                {
                    ErrorLogger.LogErrorBasedOnSeverity(bookException.Severity, bookException.Message, bookException);
                    ConsoleMessageRenderer.RenderErrorMessage(bookException.Message);
                    ConsoleMessageRenderer.RenderErrorMessage(bookException.ResolutionSuggestion);
                }
                catch (Exception exception)
                {
                    ErrorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, exception.Message, exception);
                    ConsoleMessageRenderer.RenderErrorMessage(exception.Message);
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold italic]Canceled.[/]");
            }
        }
        catch (Exception ex)
        {
            ErrorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, ex.Message, ex);
            AnsiConsole.MarkupLine($"[bold italic red]Error: {ex.Message}[/]");
        }

        AppPartialsRenderer.RenderConfirmationToContinue();
    }
}