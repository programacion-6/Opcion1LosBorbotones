using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BookOptions
{
    private readonly IBookRepository _bookRepository;

    public BookOptions(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
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
                    //TODO
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

        Guid bookId = Guid.NewGuid();
        string bookTitle = AnsiConsole.Ask<string>("Enter the book title: ");
        string bookAuthor = AnsiConsole.Ask<string>("Enter the book author: ");
        long bookIsbn = AnsiConsole.Ask<long>("Enter the book ISBN: ");
        DateTime bookPublicationYear = AnsiConsole.Ask<DateTime>("Enter the published year (yyyy/MM/dd): ");
        string bookGenre = AnsiConsole.Ask<string>("Enter the book genre: ");

        AnsiConsole.MarkupLine("[bold green]Review the book details before confirming:[/]");
        AnsiConsole.MarkupLine($"[bold]Title:[/] {bookTitle}");
        AnsiConsole.MarkupLine($"[bold]Author:[/] {bookAuthor}");
        AnsiConsole.MarkupLine($"[bold]ISBN:[/] {bookIsbn}");
        AnsiConsole.MarkupLine($"[bold]Published Year:[/] {bookPublicationYear:yyyy/MM/dd}");
        AnsiConsole.MarkupLine($"[bold]Genre:[/] {bookGenre}");

        var confirm = AnsiConsole.Confirm("Do you want to save this book?");
        if (confirm)
        {
            Book newBook = new Book(bookId, bookTitle, bookAuthor, bookIsbn, bookGenre, bookPublicationYear);
            await _bookRepository.Save(newBook);
            AnsiConsole.MarkupLine($"[bold italic green]New book registered:[/] {newBook}");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic red]Book registration canceled.[/]");
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    public async Task DeleteBook()
    {
        try
        {
            var books = (await _bookRepository.GetAll()).ToArray();

            if (books == null || books.Length == 0)
            {
                AnsiConsole.MarkupLine("[red]There are no books available for deletion.[/]");
                return;
            }

            var bookToDelete = AnsiConsole.Prompt(
                new SelectionPrompt<Book>()
                    .Title("Select the book you want to delete:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Scroll up and down to see more options)[/]")
                    .AddChoices(books)
                    .UseConverter(book => $"{book.Title} | {book.Author}")
            );

            var wasConfirmed = AnsiConsole.Confirm($"Are you sure you want to delete this book? [yellow]{bookToDelete.Title}[/]?");
            if (wasConfirmed)
            {
                await _bookRepository.Delete(bookToDelete.Isbn);
                AnsiConsole.MarkupLine("[green]The book has been successfully deleted.[/]");
                AnsiConsole.Markup("[blue] Press Enter to go back to the Book Menu.[/]");
                Console.ReadLine();
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            }
        }
        catch (System.Exception)
        {
            AnsiConsole.Markup($"[red] The book cannot be deleted, try another book.[/]\n");
            AnsiConsole.Markup("[blue] Press Enter to go back to the Book Menu.[/]");
            Console.ReadLine();
        }

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
                    "3. Search Book By ISBN",
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
            case "3. Search Book By ISBN":
                await SearchByIsbn();
                break;
        }
    }

    private async Task PaginatedSearchByTitle()
    {
        string bookTitle = AnsiConsole.Ask<string>("Book title: ");
        int page = 0;
        const int pageSize = 10;

        while (true)
        {
            var books = await _bookRepository.GetBooksByTitle(bookTitle, page * pageSize, pageSize);

            AnsiConsole.MarkupLine("[bold]Books:[/]");
            foreach (var book in books)
            {
                AnsiConsole.MarkupLine(book.ToString());
            }

            var navigationOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Navigate:[/]")
                    .AddChoices(new[] {
                    "Next Page",
                    "Previous Page",
                    "Exit"
                    })
            );

            if (navigationOption == "Next Page")
            {
                page++;
            }
            else if (navigationOption == "Previous Page" && page > 0)
            {
                page--;
            }
            else
            {
                break;
            }
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    private async Task PaginatedSearchByAuthor()
    {
        string bookAuthor = AnsiConsole.Ask<string>("Book author: ");
        int page = 0;
        const int pageSize = 10;

        while (true)
        {
            var books = await _bookRepository.GetBooksByAuthor(bookAuthor, page * pageSize, pageSize);

            AnsiConsole.MarkupLine("[bold]Books:[/]");
            foreach (var book in books)
            {
                AnsiConsole.MarkupLine(book.ToString());
            }

            var navigationOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Navigate:[/]")
                    .AddChoices(new[] {
                    "Next Page",
                    "Previous Page",
                    "Exit"
                    })
            );

            if (navigationOption == "Next Page")
            {
                page++;
            }
            else if (navigationOption == "Previous Page" && page > 0)
            {
                page--;
            }
            else
            {
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