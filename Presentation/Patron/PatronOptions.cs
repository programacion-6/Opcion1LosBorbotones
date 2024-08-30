using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Domain.Validator;
using Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class PatronOptions
{
    private readonly IPatronRepository _patronRepository;
    private readonly IEntityRequester<Patron> _patronRequester;
    private readonly PatronValidator _patronValidator;

    public PatronOptions(IPatronRepository patronRepository, IEntityRequester<Patron> patronRequester)
    {
        _patronRepository = patronRepository;
        _patronRequester = patronRequester;
        _patronValidator = new PatronValidator();
    }

    public async Task PatronInitialOptions()
    {
        bool goBack = false;
        while (!goBack)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Patron Menu[/]");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "1. Register a new patron",
                        "2. Delete a patron",
                        "3. Edit a patron",
                        "4. Search patron",
                        "5. Go back"
                    })
            );

            switch (option)
            {
                case "1. Register a new patron":
                    await RegisterNewPatron();
                    break;
                case "2. Delete a patron":
                    await DeletePatron();
                    break;
                case "3. Edit a patron":
                    await EditPatron();
                    break;
                case "4. Search patron":
                    await SearchPatron();
                    break;
                case "5. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private async Task RegisterNewPatron()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Register a new patron[/]");

        try
        {
            var newPatron = _patronRequester.AskForEntity();
            _patronValidator.ValidatePatron(newPatron);
            await _patronRepository.Save(newPatron);
            AnsiConsole.MarkupLine($"[bold italic green]New patron registered:[/] {newPatron}");
        }
        catch (PatronException patronException)
        {
            var errorMessage = $"[red bold]:warning: {patronException.Message} \n...{patronException.ResolutionSuggestion} [/]";
            AnsiConsole.MarkupLine(errorMessage);
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[bold italic red]Error: {e.Message}[/]");
        }

        AnsiConsole.Markup("[blue] Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }

    private async Task DeletePatron()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Deleted a patron[/]");

        var patronId = AnsiConsole.Ask<Guid>("Enter the patron id: ");
        var wasConfirmed = AnsiConsole.Confirm("Are you sure you want to delete this patron?");

        if (wasConfirmed)
        {
            await _patronRepository.Delete(patronId);
            AnsiConsole.MarkupLine("[bold italic red]Patron deleted.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic]Canceled.[/]");
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }

    private async Task EditPatron()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Edit patron[/]");

        var patronId = AnsiConsole.Ask<Guid>("Enter the patron id: ");
        var wasConfirmed = AnsiConsole.Confirm("Are you sure you want to edit this patron?");

        if (wasConfirmed)
        {
            try
            {
                var editedPatron = _patronRequester.AskForEntity();
                editedPatron.Id = patronId;
                _patronValidator.ValidatePatron(editedPatron);
                await _patronRepository.Update(editedPatron);
                AnsiConsole.MarkupLine($"[bold italic green]Patron edited:[/] {editedPatron}");
            }
            catch (PatronException patronException)
            {
                var errorMessage = $"[red bold]:warning: {patronException.Message} \n...{patronException.ResolutionSuggestion} [/]";
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

        AnsiConsole.Markup("[blue]Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }

    private async Task SearchPatron()
    {
        AnsiConsole.Clear();
        Header.AppHeader();

        AnsiConsole.MarkupLine("[bold yellow]Patron Searcher[/]");

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]Chose an option:[/]")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "1. Search Patron By Name",
                    "2. Search Patron By Contact Details",
                    "3. Search Patron By Membership Number",
                    "4. Go back"
                })
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
        string patronName = AnsiConsole.Ask<string>("Patron name: ");
        List<Patron> _results = [];
        var pageSize = 1;
        var currentPage = 1;
        var exit = false;

        while (!exit)
        {
            var patrons = await _patronRepository.GetPatronsByNameAsync(patronName, pageSize, currentPage * pageSize);

            if (!patrons.Any() || patrons.Count() == _results.Count())
            {
                exit = true;
                break;
            }

            AnsiConsole.Clear();
            foreach (var result in patrons)
            {
                if (!_results.Any(r => r.Id == result.Id))
                {
                    _results.Add(result);
                }
            }

            AnsiConsole.MarkupLine("[bold]Patrons:[/]");
            foreach (var patron in patrons)
            {
                AnsiConsole.MarkupLine(patron.ToString());
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

        AnsiConsole.Markup("[blue]Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }

    private async Task PaginatedSearchByContactDetails()
    {
        long patronContact = AnsiConsole.Ask<long>("Enter the contact details [bold green]BO[/]: ");
        var patronFound = await _patronRepository.GetPatronByContactDetailsAsync(patronContact);

        AnsiConsole.MarkupLine("[bold]Patron:[/]");
        AnsiConsole.MarkupLine($"{patronFound}");

        AnsiConsole.Markup("[blue]Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }

    private async Task SearchPatronsByMembershipNumber()
    {
        long patronMembershipNumber = AnsiConsole.Ask<long>("Enter the membership number: ");
        var patronByMembershipNumber = await _patronRepository.GetPatronByMembershipAsync(patronMembershipNumber);

        AnsiConsole.MarkupLine("[bold]Patron:[/]");
        AnsiConsole.MarkupLine($"{patronByMembershipNumber}");

        AnsiConsole.Markup("[blue]Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }
}
