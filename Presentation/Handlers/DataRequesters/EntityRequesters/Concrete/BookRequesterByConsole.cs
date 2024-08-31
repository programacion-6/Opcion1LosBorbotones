using Opcion1LosBorbotones.Domain;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Handlers;

public class BookRequesterByConsole : IEntityRequester<Book>
{
    private const Book? _unrequestedBook = null;

    public Book AskForEntity()
    {
        Book? requestedBook = _unrequestedBook;

        while (requestedBook is _unrequestedBook)
        {
            requestedBook = ReceiveBookByConsole();
            requestedBook = ConfirmBookReceived(requestedBook);
        }

        return requestedBook;
    }

    private Book ReceiveBookByConsole()
    {
        var id = Guid.NewGuid();
        var bookTitle = AnsiConsole.Ask<string>("Enter the title: ");
        var bookAuthor = AnsiConsole.Ask<string>("Enter the author: ");
        var bookIsbn = AnsiConsole.Ask<long>("Enter the ISBN: ");
        var bookPublicationYear = AnsiConsole.Ask<DateTime>("Enter the published year (yyyy/MM/dd): ");
        var bookGenre = AnsiConsole.Ask<string>("Enter the genre: ");

        var bookReceived = new Book(id, bookTitle, bookAuthor, bookIsbn, bookGenre, bookPublicationYear);
        return bookReceived;
    }

    private Book? ConfirmBookReceived(Book bookReceived)
    {
        RenderBookReceived(bookReceived);
        var wasConfirmed = AnsiConsole.Confirm("[bold] Do you want to continue? [/]");
        if (!wasConfirmed)
        {
            AnsiConsole.MarkupLine("[bold italic cyan]Insert the data again[/]");
            return _unrequestedBook;
        }

        return bookReceived;
    }

    private void RenderBookReceived(Book bookReceived)
    {
        AnsiConsole.MarkupLine("[bold green]Review the book details before confirming:[/]");
        AnsiConsole.MarkupLine($"[bold]Title:[/] {bookReceived.Title}");
        AnsiConsole.MarkupLine($"[bold]Author:[/] {bookReceived.Author}");
        AnsiConsole.MarkupLine($"[bold]ISBN:[/] {bookReceived.Isbn}");
        AnsiConsole.MarkupLine($"[bold]Published Year:[/] {bookReceived.PublicationYear:yyyy/MM/dd}");
        AnsiConsole.MarkupLine($"[bold]Genre:[/] {bookReceived.Genre}");
    }

}
