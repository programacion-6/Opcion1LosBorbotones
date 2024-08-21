using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Searcher;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BookOptions
{
    public static void BookInitialOptions()
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

            var repository = BookRepositoryImplementation.GetInstance();

            switch (option)
            {
                case "1. Register a new book":
                    RegisterNewBook(repository);
                    break;
                case "2. Delete a book":
                    DeleteBook(repository);
                    break;
                case "3. Edit a book":
                    //TODO
                    break;
                case "4. Search books":
                    SearchBook();
                    break;
                case "5. Go back":
                    goBack = true;
                    break; 
            }
        }
    }

    private static void RegisterNewBook(BookRepositoryImplementation repository)
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Register a new book[/]");

        Guid bookId = Guid.NewGuid();
        string bookTitle = AnsiConsole.Ask<string>("Enter the book title: ");
        string bookAuthor = AnsiConsole.Ask<string>("Enter the book author: ");
        long bookIsbn = AnsiConsole.Ask<long>("Enter the book ISBN: ");
        DateTime bookPublicationYear = AnsiConsole.Ask<DateTime>("Enter the published year (yyyy/MM/dd): ");

        BookGenre bookGenre = AnsiConsole.Prompt(
            new SelectionPrompt<BookGenre>()
                .Title("Select the book genre:")
                .AddChoices(Enum.GetValues<BookGenre>())
        );

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
            repository.CreateAsync(newBook).GetAwaiter().GetResult();
            AnsiConsole.MarkupLine($"[bold italic green]New book registered:[/] {newBook}");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic red]Book registration canceled.[/]");
        }
        
        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    private static void DeleteBook(BookRepositoryImplementation repository)
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold red]Delete a book[/]");

        string bookId = AnsiConsole.Ask<string>("Enter the book ID: ");
        var confirm = AnsiConsole.Confirm("Are you sure you want to delete this book?");
        if (confirm)
        {
            Guid bookUUID = new Guid(bookId);
            repository.DeleteAsync(bookUUID).GetAwaiter().GetResult();
            AnsiConsole.MarkupLine("[bold italic red]Book deleted.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic]Canceled.[/]");
        }
        
        AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
        Console.ReadLine();
    }

    private static void SearchBook()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        BookSearcher bookSearcher = new BookSearcher();

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
                string bookTitle = AnsiConsole.Ask<string>("Book title: ");
                var booksByTitle = bookSearcher.SearchBookByTile(bookTitle).GetAwaiter().GetResult();

                AnsiConsole.MarkupLine("[bold]Books:[/]");
                foreach (var book in booksByTitle)
                {
                    AnsiConsole.MarkupLine(book.ToString());
                }

                AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
                Console.ReadLine();
                break;
            case "2. Search Book By Author":
                string bookAuthor = AnsiConsole.Ask<string>("Book author: ");
                var booksByAuthor = bookSearcher.SearchBookByAuthor(bookAuthor).GetAwaiter().GetResult();
                
                AnsiConsole.MarkupLine("[bold]Books:[/]");
                foreach (var book in booksByAuthor)
                {
                    AnsiConsole.MarkupLine(book.ToString());
                }
                
                AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
                Console.ReadLine();
                break;
            case "3. Search Book By ISBN":
                
                long Isbn = AnsiConsole.Ask<long>("Book ISBN: ");
                var bookByIsbn = bookSearcher.SearchBookByIsbn(Isbn).GetAwaiter().GetResult();
                
                AnsiConsole.MarkupLine("[bold]Book:[/]");
                AnsiConsole.MarkupLine(bookByIsbn.ToString());
                
                AnsiConsole.Markup("[blue]Press Enter to go back to the Book Menu.[/]");
                Console.ReadLine();
                break;
        }
    }
}