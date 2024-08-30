using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class PatronOptions
{
    private readonly IPatronRepository _patronRepository;
    private readonly IEntityRequester<Patron> _patronRequester;

    public PatronOptions(IPatronRepository patronRepository, IEntityRequester<Patron> patronRequester)
    {
        _patronRepository = patronRepository;
        _patronRequester = patronRequester;
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
            await _patronRepository.Save(newPatron);
            AnsiConsole.MarkupLine($"[bold italic green]New patron registered:[/] {newPatron}");
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

        long patronMembershipNumber = AnsiConsole.Ask<long>("Enter the patron member number: ");
        var wasConfirmed = AnsiConsole.Confirm("Are you sure you want to delete this patron?");

        if (wasConfirmed)
        {
            long patron = patronMembershipNumber;
            await _patronRepository.Delete(patron);
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
                await _patronRepository.Update(editedPatron);
                AnsiConsole.MarkupLine($"[bold italic green]Patron edited:[/] {editedPatron}");
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
                    "2. Search Patron By Membership Number",
                    "4. Go back"
                })
        );

        switch (option)
        {
            case "1. Search Patron By Name":
                await PaginatedSearchByName();
                break;
            case "2. Search Patron By Membership Number":
                await SearchPatronsByMembershipNumber();
                break;
        }
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

    private async Task PaginatedSearchByName()
    {
        string patronName = AnsiConsole.Ask<string>("Patron name: ");
        int page = 0;
        const int pageSize = 10;

        while (true)
        {
            var patrons = await _patronRepository.GetPatronsByNameAsync(patronName, page * pageSize, pageSize);

            AnsiConsole.MarkupLine("[bold]Patrons:[/]");
            foreach (var patron in patrons)
            {
                AnsiConsole.MarkupLine(patron.ToString());
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
}
