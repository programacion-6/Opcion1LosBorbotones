using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Domain.Validator;
using Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;
using Opcion1LosBorbotones.Presentation.Handlers;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Executors;

public class PatronHandlerExecutor : IExecutor
{
    private readonly IPatronRepository _patronRepository;
    private readonly IEntityRequester<Patron> _patronRequester;
    private readonly PatronValidator _patronValidator;
    private readonly PatronFinderExecutor _patronFinder;

    public PatronHandlerExecutor(IPatronRepository patronRepository, IEntityRequester<Patron> patronRequester, PatronFinderExecutor patronFinder)
    {
        _patronRepository = patronRepository;
        _patronRequester = patronRequester;
        _patronFinder = patronFinder;
        _patronValidator = new();
    }

    public async Task Execute()
    {
        bool goBack = false;
        while (!goBack)
        {
            AppPartialsRenderer.RenderHeader();
            ConsoleMessageRenderer.RenderIndicatorMessage("Patron Menu");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices(
                    [
                        "1. Register a new patron",
                        "2. Delete a patron",
                        "3. Edit a patron",
                        "4. Search patron",
                        "5. Go back"
                    ])
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
                    await _patronFinder.Execute();
                    break;
                case "5. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private async Task RegisterNewPatron()
    {
        AppPartialsRenderer.RenderHeader();
        ConsoleMessageRenderer.RenderIndicatorMessage("Register a new patron");

        try
        {
            var newPatron = _patronRequester.AskForEntity();
            _patronValidator.ValidatePatron(newPatron);
            await _patronRepository.Save(newPatron);
            ConsoleMessageRenderer.RenderSuccessMessage("New patron registered");
        }
        catch (BookException bookException)
        {
            ConsoleMessageRenderer.RenderErrorMessage(bookException.Message);
            ConsoleMessageRenderer.RenderErrorMessage(bookException.ResolutionSuggestion);
        }
        catch (Exception exception)
        {
            ConsoleMessageRenderer.RenderErrorMessage(exception.Message);
        }

        AppPartialsRenderer.RenderConfirmationToContinue();
    }

    private async Task DeletePatron()
    {
        AppPartialsRenderer.RenderHeader();
        AnsiConsole.MarkupLine("[bold yellow]Deleted a patron[/]");

        var patronId = AnsiConsole.Ask<Guid>("Enter the patron id: ");
        var wasConfirmed = AnsiConsole.Confirm("Are you sure you want to delete this patron?");

        if (wasConfirmed)
        {
            try
            {
                await _patronRepository.Delete(patronId);
                ConsoleMessageRenderer.RenderSuccessMessage("Patron deleted");
            }
            catch (BookException bookException)
            {
                ConsoleMessageRenderer.RenderErrorMessage(bookException.Message);
                ConsoleMessageRenderer.RenderErrorMessage(bookException.ResolutionSuggestion);
            }
            catch (Exception exception)
            {
                ConsoleMessageRenderer.RenderErrorMessage(exception.Message);
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic]Canceled.[/]");
        }

        AppPartialsRenderer.RenderConfirmationToContinue();
    }

    private async Task EditPatron()
    {
        AppPartialsRenderer.RenderHeader();
        ConsoleMessageRenderer.RenderIndicatorMessage("Edit patron");
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
                ConsoleMessageRenderer.RenderSuccessMessage("Patron edited");
            }
            catch (BookException bookException)
            {
                ConsoleMessageRenderer.RenderErrorMessage(bookException.Message);
                ConsoleMessageRenderer.RenderErrorMessage(bookException.ResolutionSuggestion);
            }
            catch (Exception exception)
            {
                ConsoleMessageRenderer.RenderErrorMessage(exception.Message);
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[bold italic]Canceled.[/]");
        }

        AppPartialsRenderer.RenderConfirmationToContinue();
    }


}
