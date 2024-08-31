using Opcion1LosBorbotones.Domain;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class UserDrivenPagedSearcher<T, I>
{
    private int _numberOfResultsFound;
    private readonly ISearchStrategy<T, I> _searchStrategy;

    public UserDrivenPagedSearcher(ISearchStrategy<T, I> searchStrategy)
    {
        _searchStrategy = searchStrategy;
    }

    public async Task ExecuteSearchAsync()
    {
        var bookTitle = AnsiConsole.Ask<I>($"[bold] {_searchStrategy.GetPrompt()}: [/]");
        var pageSize = 1;
        var currentPage = 1;
        var exit = false;

        while (!exit)
        {
            var searchResults = await _searchStrategy.SearchByPage(bookTitle, pageSize, currentPage * pageSize);
            var thereAreNoResults = searchResults.Count == 0 ||
                        searchResults.Count == _numberOfResultsFound;

            if (thereAreNoResults)
            {
                var noResultsWereFound = _numberOfResultsFound == 0;
                if (noResultsWereFound)
                {
                    AnsiConsole.MarkupLine("[bold italic yellow]No results found:[/]");
                }
                exit = true;
                break;
            }

            _numberOfResultsFound = 0;
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold cyan]Results:[/]");

            foreach (var result in searchResults)
            {
                AnsiConsole.MarkupLine($"{result}");
                _numberOfResultsFound++;
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

        AnsiConsole.Markup("[blue]Press Enter to continue.[/]");
        Console.ReadLine();
    }
}
