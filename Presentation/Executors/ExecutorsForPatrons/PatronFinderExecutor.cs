
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Searchers.PatronSearchers;
using Opcion1LosBorbotones.Presentation.Handlers;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Executors;

public class PatronFinderExecutor : IExecutor
{
    private readonly IPatronRepository _patronRepository;

    public PatronFinderExecutor(IPatronRepository patronRepository)
    {
        _patronRepository = patronRepository;
    }

    public async Task Execute()
    {
        AppPartialsRenderer.RenderHeader();
        ConsoleMessageRenderer.RenderIndicatorMessage("Patron Searcher");

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]Chose an option:[/]")
                .PageSize(10)
                .AddChoices(
                [
                    "1. Search Patron By Name",
                    "2. Search Patron By Contact Details",
                    "3. Search Patron By Membership Number",
                    "4. Go back"
                ])
        );

        switch (option)
        {
            case "1. Search Patron By Name":
                await PaginatedSearchByName();
                break;
            case "2. Search Patron By Contact Details":
                await PaginatedSearchByContactDetails();
                break;
            case "3. Search Patron By Membership Number":
                await SearchPatronsByMembershipNumber();
                break;
        }
    }

    private async Task PaginatedSearchByName()
    {
        var prompt = "Enter the name";
        var criteriaRequester = new PromptRequester<string>(prompt);
        var searchStrategy = new SearcherByName(_patronRepository);
        var searchService = new UserDrivenPagedSearcher<Patron, string>(searchStrategy, criteriaRequester);
        await searchService.ExecuteSearchAsync();
    }

    private async Task PaginatedSearchByContactDetails()
    {
        long contact = AnsiConsole.Ask<long>("Enter the contact details: ");
        var patronFound = await _patronRepository.GetPatronByContactDetailsAsync(contact);
        ConsoleMessageRenderer.RenderIndicatorMessage("Patron found");
        ResultRenderer.RenderResult(patronFound);
        AppPartialsRenderer.RenderConfirmationToContinue();
    }

    private async Task SearchPatronsByMembershipNumber()
    {
        long membershipNumber = AnsiConsole.Ask<long>("Enter the membership number: ");
        var patronFound = await _patronRepository.GetPatronByMembershipAsync(membershipNumber);
        ConsoleMessageRenderer.RenderIndicatorMessage("Patron found");
        ResultRenderer.RenderResult(patronFound);
        AppPartialsRenderer.RenderConfirmationToContinue();
    }
}