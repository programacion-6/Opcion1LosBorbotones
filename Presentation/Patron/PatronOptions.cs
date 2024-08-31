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

    private IEntityFormatterFactory<Patron> _formatterFactoryBorrow;

    public PatronOptions(IPatronRepository patronRepository,
                        IEntityRequester<Patron> patronRequester,
                        IEntityFormatterFactory<Patron> formatterFactoryBorrow)
    {
        _patronRepository = patronRepository;
        _patronRequester = patronRequester;
        _patronValidator = new PatronValidator();
        _formatterFactoryBorrow = formatterFactoryBorrow;
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
            var formatter = await _formatterFactoryBorrow.CreateDetailedFormatter(newPatron);

            AnsiConsole.MarkupLine($"[bold italic green]New patron registered:[/]\n {formatter}");
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
        AnsiConsole.MarkupLine("[bold yellow]Delete a patron[/]");

        try
        {
            var patrons = (await _patronRepository.GetAll()).ToArray();

            if (patrons == null || patrons.Length == 0)
            {
                AnsiConsole.MarkupLine("[red]No patrons available for deletion.[/]");
                return;
            }

            var patronToDelete = AnsiConsole.Prompt(
                new SelectionPrompt<Patron>()
                    .Title("Select the patron you want to delete:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Scroll up and down to see more options)[/]")
                    .AddChoices(patrons)
                    .UseConverter(patron => $"{patron.Name} | {patron.ContactDetails} | {patron.MembershipNumber}")
            );

            var wasConfirmed = AnsiConsole.Confirm($"Are you sure you want to delete this patron? [yellow]{patronToDelete.Name}[/]");

            if (wasConfirmed)
            {
                try
                {
                    await _patronRepository.Delete(patronToDelete.MembershipNumber);
                    AnsiConsole.MarkupLine("[bold italic red]Patron deleted.[/]");
                }
                catch (Exception e)
                {
                    AnsiConsole.MarkupLine("[bold italic red]An unexpected error occurred. Please try again later.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold italic]Canceled.[/]");
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine("[bold italic red]An unexpected error occurred. Please try again later.[/]");
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }

    private async Task EditPatron()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Edit patron[/]");

        try
        {
            var patrons = (await _patronRepository.GetAll()).ToArray();

            if (patrons == null || patrons.Length == 0)
            {
                AnsiConsole.MarkupLine("[red]No patrons available for editing.[/]");
                return;
            }

            var patronToEdit = AnsiConsole.Prompt(
                new SelectionPrompt<Patron>()
                    .Title("Select the patron you want to edit:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Scroll up and down to see more options)[/]")
                    .AddChoices(patrons)
                    .UseConverter(patron => $"{patron.Name} | {patron.ContactDetails} | {patron.MembershipNumber}")
            );

            var wasConfirmed = AnsiConsole.Confirm($"Are you sure you want to edit this patron? [yellow]{patronToEdit.Name}[/]");

            if (wasConfirmed)
            {
                try
                {
                    var editedPatron = _patronRequester.AskForEntity();
                    editedPatron.Id = patronToEdit.Id;
                    _patronValidator.ValidatePatron(editedPatron);
                    await _patronRepository.Update(editedPatron);
                    var formatter = await _formatterFactoryBorrow.CreateDetailedFormatter(editedPatron);

                    AnsiConsole.MarkupLine($"[bold italic green]Patron edited:[/]\n {formatter}");

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
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[bold italic red]Error: {e.Message}[/]");
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
        var formatter = await _formatterFactoryBorrow.CreateDetailedFormatter(patronByMembershipNumber);

        AnsiConsole.MarkupLine($"{formatter}");

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
                var formatter = await _formatterFactoryBorrow.CreateDetailedFormatter(patron);

                AnsiConsole.MarkupLine($"{formatter}");
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
