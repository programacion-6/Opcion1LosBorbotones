using Opcion1LosBorbotones.Infrastructure.Services.Borrows;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Executors;

public class LoanHandlerExecutor : IExecutor
{
    private readonly IBorrowService _borrowService;
    private readonly IBorrowConsoleRenderer _borrowConsoleRenderer;

    public LoanHandlerExecutor(IBorrowService borrowService)
    {
        _borrowService = borrowService;
        _borrowConsoleRenderer = new BorrowConsoleRenderer();
    }

    public async Task Execute()
    {
        bool goBack = false;
        while (!goBack)
        {
            AppPartialsRenderer.RenderHeader();
            ConsoleMessageRenderer.RenderIndicatorMessage("Borrow Menu");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices(
                    [
                        "1. Request a borrow",
                        "2. Go back"
                    ])
            );

            switch (option)
            {
                case "1. Request a borrow":
                    await RegisterNewBorrow();
                    break;
                case "2. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private async Task RegisterNewBorrow()
    {
        try
        {
            AppPartialsRenderer.RenderHeader();
            ConsoleMessageRenderer.RenderIndicatorMessage("New loan");
            var patronUUID = _borrowConsoleRenderer.GetPatronId();
            var bookUUID = _borrowConsoleRenderer.GetBookId();
            var borrow = await _borrowService.RegisterNewBorrow(patronUUID, bookUUID);
            _borrowConsoleRenderer.DisplayBorrowDetails(borrow);

            if (_borrowConsoleRenderer.ConfirmBorrow())
            {
                ConsoleMessageRenderer.RenderSuccessMessage($"New borrow registered {borrow}");
            }
            else
            {
                ConsoleMessageRenderer.RenderInfoMessage("No borrow registered");
            }

            AppPartialsRenderer.RenderConfirmationToContinue();
        }
        catch (Exception)
        {
            ConsoleMessageRenderer.RenderErrorMessage("The data entered is not correct, please enter correct data");
            AppPartialsRenderer.RenderConfirmationToContinue();
        }
    }
}