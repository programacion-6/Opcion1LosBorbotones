using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Searchers.LoanSearchers;
using Opcion1LosBorbotones.Presentation.Handlers;
using Opcion1LosBorbotones.Presentation.Renderer.BorrowFormatter;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Executors;

public class ReportHandlerExecutor : IExecutor
{
    private readonly IBorrowRepository _borrowRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;
    private readonly Func<Borrow, string> _detailedBorrowFormatter;

    public ReportHandlerExecutor(IBorrowRepository borrowRepository, IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _borrowRepository = borrowRepository;
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
        _detailedBorrowFormatter = b => new DetailedBorrowFormatter(b, _bookRepository, _patronRepository).ToString();
    }

    public async Task Execute()
    {
        bool goBack = false;
        while (!goBack)
        {
            AppPartialsRenderer.RenderHeader();
            ConsoleMessageRenderer.RenderIndicatorMessage("Report Menu");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices(
                    [
                        "1. Report books currently borrowed",
                        "2. Report overdue books",
                        "3. Borrowing history for a patron",
                        "4. Go back"
                    ])
            );

            switch (option)
            {
                case "1. Report books currently borrowed":
                    await ReportBooksCurrentlyBorrowed();
                    break;
                case "2. Report overdue books":
                    await ReportBooksOverdue();
                    break;
                case "3. Borrowing history for a patron":
                    await ReportPatronBorrowed();
                    break;
                case "4. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private async Task ReportBooksCurrentlyBorrowed()
    {
        var defaultSearchCriteria = new DefaultSearchCriteria<BorrowStatus>(BorrowStatus.Borrowed);
        var searchStrategy = new LoanSearcherByState(_borrowRepository);
        var searchService = new UserDrivenPagedSearcher<Borrow, BorrowStatus>(
                                searchStrategy, 
                                defaultSearchCriteria,
                                _detailedBorrowFormatter
                                );
        await searchService.ExecuteSearchAsync();
    }

    private async Task ReportBooksOverdue()
    {
        var defaultSearchCriteria = new DefaultSearchCriteria<BorrowStatus>(BorrowStatus.Overdue);
        var searchStrategy = new LoanSearcherByState(_borrowRepository);
        var searchService = new UserDrivenPagedSearcher<Borrow, BorrowStatus>(
                                searchStrategy, 
                                defaultSearchCriteria,
                                _detailedBorrowFormatter
                                );
        await searchService.ExecuteSearchAsync();
    }

    private async Task ReportPatronBorrowed()
    {
        var prompt = "Enter the Patron MembershipNumber:";
        var criteriaRequester = new PromptRequester<long>(prompt);
        var searchStrategy = new SearcherForLoansbyPatron(_borrowRepository);
        var searchService = new UserDrivenPagedSearcher<Borrow, long>(
                                searchStrategy, 
                                criteriaRequester,
                                _detailedBorrowFormatter
                                );
        await searchService.ExecuteSearchAsync();
    }
}