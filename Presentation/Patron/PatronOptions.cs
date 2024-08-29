using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class PatronOptions
{
    private readonly IPatronRepository _patronRepository;

    public PatronOptions(IPatronRepository patronRepository)
    {
        _patronRepository = patronRepository;
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

        Guid patronId = Guid.NewGuid();
        string patronName = AnsiConsole.Ask<string>("Enter the patron name: ");
        long patronMembershipNumber = AnsiConsole.Ask<long>("Enter the membership number: ");
        long patronContactDetailNumber = AnsiConsole.Ask<long>("Enter the contact detail number: ");

        AnsiConsole.MarkupLine("[bold green]Review the PATRON details before confirming:[/]");
        AnsiConsole.MarkupLine($"[bold] Name [/]: {patronName}");
        AnsiConsole.MarkupLine($"[bold] Membership number [/]: {patronMembershipNumber}");
        AnsiConsole.MarkupLine($"[bold] Contact details [/]: {patronContactDetailNumber}");

        var confirm = AnsiConsole.Confirm("[bold] Do you want to register this patron? [/]");

        if (confirm)
        {
            Patron newPatron = new Patron(patronId, patronName, patronMembershipNumber, patronContactDetailNumber);
            await _patronRepository.Save(newPatron);
            AnsiConsole.MarkupLine($"[bold italic green]New patron registered:[/] {newPatron}");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic red]Patron registration canceled.[/]");
        }

        AnsiConsole.Markup("[blue] Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }

    private async Task DeletePatron()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Deleted a patron[/]");

        string patronId = AnsiConsole.Ask<string>("Enter the patron id: ");
        var confirm = AnsiConsole.Confirm("Are you sure you want to delete this patron?");

        if (confirm)
        {
            Guid patronUUID = new Guid(patronId);
            await _patronRepository.Delete(patronUUID);
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

        string patronId = AnsiConsole.Ask<string>("Enter the patron id: ");
        var confirm = AnsiConsole.Confirm("Are you sure you want to delete this patron?");

        if (confirm)
        {
            Guid patronUUID = new Guid(patronId);
            string patronName = AnsiConsole.Ask<string>("Enter the patron name: ");
            long patronMembershipNumber = AnsiConsole.Ask<long>("Enter the membership number: ");
            long patronContactDetailNumber = AnsiConsole.Ask<long>("Enter the contact detail number: ");

            AnsiConsole.MarkupLine("[bold green]Review the PATRON details before confirming:[/]");
            AnsiConsole.MarkupLine($"[bold] Name [/]: {patronName}");
            AnsiConsole.MarkupLine($"[bold] Membership number [/]: {patronMembershipNumber}");
            AnsiConsole.MarkupLine($"[bold] Contact details [/]: {patronContactDetailNumber}");

            var editingConfirmation = AnsiConsole.Confirm("[bold] Do you want to save the update? [/]");

            if (editingConfirmation)
            {
                Patron newPatron = new Patron(patronUUID, patronName, patronMembershipNumber, patronContactDetailNumber);
                await _patronRepository.Update(newPatron);
                AnsiConsole.MarkupLine($"[bold italic green]Updated:[/] {newPatron}");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold italic red]Patron edition cancelled.[/]");
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
