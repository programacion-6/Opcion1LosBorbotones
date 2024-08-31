using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Presentation.Handlers;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class UserDrivenPagedSearcher<T, I>
{
    private int _numberOfResultsFound;
    private readonly ISearchStrategy<T, I> _searchStrategy;
    private readonly ISearchCriteriaRequester<I> _criteriaRequester;

    public UserDrivenPagedSearcher(ISearchStrategy<T, I> searchStrategy, ISearchCriteriaRequester<I> criteriaRequester)
    {
        _searchStrategy = searchStrategy;
        _criteriaRequester = criteriaRequester;
    }

    public async Task ExecuteSearchAsync()
    {
        var criteria = _criteriaRequester.RequestCriteria();
        var pageSize = 1;
        var currentPage = 1;
        var isValidToRun = true;

        while (isValidToRun)
        {
            var searchResults = await PerformSearchAsync(criteria, pageSize, currentPage);
            isValidToRun = HandleSearchResults(searchResults);

            if (isValidToRun)
            {
                isValidToRun = HandleUserChoice(ref currentPage);
            }
        }

        AnsiConsole.Markup("[blue]Press Enter to continue.[/]");
        Console.ReadLine();
    }

    private async Task<List<T>> PerformSearchAsync(I criteria, int pageSize, int currentPage)
    {
        return await _searchStrategy.SearchByPage(criteria, pageSize, currentPage * pageSize);
    }

    private bool HandleSearchResults(List<T> searchResults)
    {
        var isAbleToContinue = true;
        if (NoResultsFound(searchResults))
        {
            DisplayNoResultsMessage();
            isAbleToContinue = false;
        }

        _numberOfResultsFound = searchResults.Count;
        DisplayResults(searchResults);

        return isAbleToContinue;
    }

    private bool NoResultsFound(List<T> searchResults)
    {
        return searchResults.Count == 0 || searchResults.Count == _numberOfResultsFound;
    }

    private void DisplayNoResultsMessage()
    {
        if (_numberOfResultsFound == 0)
        {
            ConsoleMessageRenderer.RenderInfoMessage("No results found");
        }
    }

    private void DisplayResults(List<T> searchResults)
    {
        AnsiConsole.Clear();
        ConsoleMessageRenderer.RenderIndicatorMessage("Results:");
        ResultRenderer.RenderResults(searchResults);
    }

    private bool HandleUserChoice(ref int currentPage)
    {
        bool isAbleToContinue;
        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                        .AddChoices("Next", "Stop"));

        switch (choice)
        {
            case "Next":
                currentPage++;
                isAbleToContinue = true;
                break;
            default:
                isAbleToContinue = false;
                break;
        }

        return isAbleToContinue;
    }
}
