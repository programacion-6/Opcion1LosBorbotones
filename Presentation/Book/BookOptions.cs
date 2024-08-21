using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BookOptions
{
    
    public static void BookInitialOptions()
    {
        AnsiConsole.Clear();
        
        Header.AppHeader();
        
        AnsiConsole.MarkupLine("[bold yellow]Book Menu[/]");

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]Chose a option:[/]")
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
                break;
            case "3. Edit a book":
                break;
            case "4. Search books":
                break;
            case "5. Go back":
                MainMenu.InitialMainMenu();
                break;
        }
    }

    private static async Task RegisterNewBook(BookRepositoryImplementation repository)
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
        
        AnsiConsole.MarkupLine($"[bold green]Review the book details before confirming:[/]");
        AnsiConsole.MarkupLine($"[bold]Title:[/] {bookTitle}");
        AnsiConsole.MarkupLine($"[bold]Author:[/] {bookAuthor}");
        AnsiConsole.MarkupLine($"[bold]ISBN:[/] {bookIsbn}");
        AnsiConsole.MarkupLine($"[bold]Published Year:[/] {bookPublicationYear:yyyy/MM/dd}");
        AnsiConsole.MarkupLine($"[bold]Genre:[/] {bookGenre}");
        
        var confirm = AnsiConsole.Confirm("Do you want to save this book?");

        if (confirm)
        {
            Book newBook = new Book(bookId, bookTitle, bookAuthor, bookIsbn, bookGenre, bookPublicationYear);
            repository.CreateAsync(newBook);
            AnsiConsole.MarkupLine($"[bold italic green]New book registered:[/] {newBook}");
            BookInitialOptions();
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic red]Book registration canceled.[/]");
            
            BookInitialOptions();
        }
    }
}