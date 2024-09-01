
using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Searchers.BookSearchers;
using Opcion1LosBorbotones.Presentation.Handlers;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Executors;

public class BookFinderExecutor : IExecutor
{
    private readonly IBookRepository _bookRepository;
    private readonly IEntityFormatterFactory<Book> _formatterFactoryBook;

    public BookFinderExecutor(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
        _formatterFactoryBook = new BookFormatterFactory();
    }

    public async Task Execute()
    {
        AppPartialsRenderer.RenderHeader();
        ConsoleMessageRenderer.RenderIndicatorMessage("Book Searcher");

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]Chose an option:[/]")
                .PageSize(10)
                .AddChoices(
                [
                    "1. Search Book By Title",
                    "2. Search Book By Author",
                    "3. Search Book By Genre",
                    "4. Search Book By ISBN",
                    "4. Go back"
                ])
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
        var prompt = "Enter the genre";
        var criteriaRequester = new PromptRequester<string>(prompt);
        var searchStrategy = new SearcherByGenre(_bookRepository);
        var searchService = new UserDrivenPagedSearcher<Book, string>(searchStrategy, criteriaRequester);
        await searchService.ExecuteSearchAsync();
    }

    private async Task PaginatedSearchByTitle()
    {
        var prompt = "Enter the title";
        var criteriaRequester = new PromptRequester<string>(prompt);
        var searchStrategy = new SearcherByTitle(_bookRepository);
        var searchService = new UserDrivenPagedSearcher<Book, string>(searchStrategy, criteriaRequester);
        await searchService.ExecuteSearchAsync();
    }

    private async Task PaginatedSearchByAuthor()
    {
        var prompt = "Enter the author";
        var criteriaRequester = new PromptRequester<string>(prompt);
        var searchStrategy = new SearcherByAuthor(_bookRepository);
        var searchService = new UserDrivenPagedSearcher<Book, string>(searchStrategy, criteriaRequester);
        await searchService.ExecuteSearchAsync();
    }

    private async Task SearchByIsbn()
    {
        var isbn = AnsiConsole.Ask<long>("Book ISBN: ");
        var book = await _bookRepository.GetBookByISBN(isbn);
        //ResultRenderer.RenderResult(book);
        _formatterFactoryBook.CreateDetailedFormatter(book);
        AppPartialsRenderer.RenderConfirmationToContinue();
    }
}