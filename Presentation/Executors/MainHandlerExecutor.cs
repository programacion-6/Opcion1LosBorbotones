using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Executors;

public class MainHandlerExecutor : IExecutor
{
    private BookHandlerExecutor _bookOptions;
    private PatronHandlerExecutor _patronOptions;
    private LoanHandlerExecutor _borrowOptions;
    private ReportHandlerExecutor _reportsOptions;

    public MainHandlerExecutor(BookHandlerExecutor bookOptions, PatronHandlerExecutor patronOptions, LoanHandlerExecutor borrowOptions, ReportHandlerExecutor reportsOptions)
    {
        _bookOptions = bookOptions;
        _patronOptions = patronOptions;
        _borrowOptions = borrowOptions;
        _reportsOptions = reportsOptions;
    }

    public async Task Execute()
    {
        bool exit = false;

        while (exit == false)
        {
            AppPartialsRenderer.RenderHeader();
            AnsiConsole.MarkupLine("[bold yellow]Menu[/]");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose a option:[/]")
                    .PageSize(10)
                    .AddChoices([
                        "1. Books",
                        "2. Patrons",
                        "3. Borrow",
                        "4. Reports",
                        "5. Exit"
                    ])
            );

            switch (option)
            {
                case "1. Books":
                    await _bookOptions.Execute();
                    break;
                case "2. Patrons":
                    await _patronOptions.Execute();
                    break;
                case "3. Borrow":
                    await _borrowOptions.Execute();
                    break;
                case "4. Reports":
                    await _reportsOptions.Execute();
                    break;
                case "5. Exit":
                    AnsiConsole.Clear();
                    exit = true;
                    break;
            }
        }
    }
}