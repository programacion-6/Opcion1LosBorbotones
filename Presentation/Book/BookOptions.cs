using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Domain.Validator;
using Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BookOptions
{
    private readonly IBookRepository _bookRepository;
    private readonly IEntityRequester<Book> _bookRequester;
    private readonly BookValidator _bookValidator;

    public BookOptions(IBookRepository bookRepository, IEntityRequester<Book> bookRequester)
    {
        _bookRepository = bookRepository;
        _bookRequester = bookRequester;
        _bookValidator = new BookValidator();
    }

    public async Task BookInitialOptions()
    {
        bool goBack = false;
        while (!goBack)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Book Menu[/]");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Register a new book",
                        "2. Delete a book",
                        "3. Edit a book",
                        "4. Search books",
                        "5. Go back"
                    })
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
                    await SearchBook();
                    break;
                case "5. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private async Task RegisterNewBook()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Register a new book[/]");

        try
        {
            var newBook = _bookRequester.AskForEntity();
            _bookValidator.ValidateBook(newBook);
            await _bookRepository.Save(newBook);
            AnsiConsole.MarkupLine($"[bold italic green]New book registered:[/] {newBook}");
        }
        catch (BookException bookException)
        {
            var errorMessage = $"[red bold]:warning: {bookException.Message} \n...{bookException.ResolutionSuggestion} [/]";
            AnsiConsole.MarkupLine(errorMessage);
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[bold italic red]Error: {e.Message}[/]");
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    private async Task DeleteBook()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold red]Delete a book[/]");

        var bookId = AnsiConsole.Ask<Guid>("Enter the book ID: ");
        var wasConfirmed = AnsiConsole.Confirm("Are you sure you want to delete this book?");
        if (wasConfirmed)
        {
            await _bookRepository.Delete(bookId);
            AnsiConsole.MarkupLine("[bold italic red]Book deleted.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic]Canceled.[/]");
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    private async Task EditBook()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Edit book[/]");

        var bookId = AnsiConsole.Ask<Guid>("Enter the book ID: ");
        var wasConfirmed = AnsiConsole.Confirm("Are you sure you want to edit this book?");

        if (wasConfirmed)
        {
            try
            {
                var editedBook = _bookRequester.AskForEntity();
                editedBook.Id = bookId;
                _bookValidator.ValidateBook(editedBook);
                await _bookRepository.Update(editedBook);
                AnsiConsole.MarkupLine($"[bold italic green]Book edited:[/] {editedBook}");
            }
            catch (BookException bookException)
            {
                var errorMessage = $"[red bold]:warning: {bookException.Message} \n...{bookException.ResolutionSuggestion} [/]";
                AnsiConsole.MarkupLine(errorMessage);
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[bold italic red]Error: {e.Message}[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic]Canceled.[/]");
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    private async Task SearchBook()
    {
        AnsiConsole.Clear();
        Header.AppHeader();

        AnsiConsole.MarkupLine("[bold yellow]Book Searcher[/]");

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]Chose an option:[/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "1. Search Book By Title",
                    "2. Search Book By Author",
                    "3. Search Book By Genre",
                    "4. Search Book By ISBN",
                    "4. Go back"
                })
        );

        switch (option)
        {
            case "1. Search Book By Title":
                await PaginatedSearchByTitle();
                break;
            case "2. Search Book By Author":
                await PaginatedSearchByAuthor();
                break;
            case "3. Search Book By Genre":
                await PaginatedSearchByGenre();
                break;
            case "4. Search Book By ISBN":
                await SearchByIsbn();
                break;
        }
    }

    private async Task PaginatedSearchByGenre()
    {
        var bookGenre = AnsiConsole.Ask<string>("Book genre: ");
        List<Book> _results = [];
        var pageSize = 1;
        var currentPage = 1;
        var exit = false;

        while (!exit)
        {
            var books = await _bookRepository.GetBooksByGenre(bookGenre, pageSize, currentPage * pageSize);

            if (!books.Any() || books.Count() == _results.Count())
            {
                exit = true;
                break;
            }

            AnsiConsole.Clear();
            foreach (var result in books)
            {
                if (!_results.Any(r => r.Id == result.Id))
                {
                    _results.Add(result);
                }
            }

            AnsiConsole.MarkupLine("[bold]Books:[/]");
            foreach (var book in books)
            {
                AnsiConsole.MarkupLine(book.ToString());
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(["Next", "Stop"])
            );

            switch (choice)
            {
                case "Next":
                    currentPage++;
                    break;

                case "Stop":
                    exit = true;
                    break;
            }
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    private async Task PaginatedSearchByTitle()
    {
        var bookTitle = AnsiConsole.Ask<string>("Book title: ");
        List<Book> _results = [];
        var pageSize = 1;
        var currentPage = 1;
        var exit = false;

        while (!exit)
        {
            var books = await _bookRepository.GetBooksByTitle(bookTitle, pageSize, currentPage * pageSize);

            if (!books.Any() || books.Count() == _results.Count())
            {
                exit = true;
                break;
            }

            AnsiConsole.Clear();
            foreach (var result in books)
            {
                if (!_results.Any(r => r.Id == result.Id))
                {
                    _results.Add(result);
                }
            }

            AnsiConsole.MarkupLine("[bold]Books:[/]");
            foreach (var book in books)
            {
                AnsiConsole.MarkupLine(book.ToString());
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(["Next", "Stop"])
            );

            switch (choice)
            {
                case "Next":
                    currentPage++;
                    break;

                case "Stop":
                    exit = true;
                    break;
            }
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    private async Task PaginatedSearchByAuthor()
    {
        string bookAuthor = AnsiConsole.Ask<string>("Book author: ");
        List<Book> _results = [];
        var pageSize = 1;
        var currentPage = 1;
        var exit = false;

        while (!exit)
        {
            var books = await _bookRepository.GetBooksByAuthor(bookAuthor, pageSize, currentPage * pageSize);

            if (!books.Any() || books.Count() == _results.Count())
            {
                exit = true;
                break;
            }

            AnsiConsole.Clear();
            foreach (var result in books)
            {
                if (!_results.Any(r => r.Id == result.Id))
                {
                    _results.Add(result);
                }
            }

            AnsiConsole.MarkupLine("[bold]Books:[/]");
            foreach (var book in books)
            {
                AnsiConsole.MarkupLine(book.ToString());
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(["Next", "Stop"])
            );

            switch (choice)
            {
                case "Next":
                    currentPage++;
                    break;

                case "Stop":
                    exit = true;
                    break;
            }
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    private async Task SearchByIsbn()
    {
        long isbn = AnsiConsole.Ask<long>("Book ISBN: ");
        var book = await _bookRepository.GetBookByISBN(isbn);

        AnsiConsole.MarkupLine("[bold]Book:[/]");
        AnsiConsole.MarkupLine(book?.ToString() ?? "Book not found.");

        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

}