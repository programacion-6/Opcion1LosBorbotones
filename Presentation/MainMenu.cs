using Opcion1LosBorbotones.Presentation.Reports;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class MainMenu
{
    private BookOptions _bookOptions;
    private PatronOptions _patronOptions;
    private BorrowOptions _borrowOptions;
    private ReportsOptions _reportsOptions;

    public MainMenu(BookOptions bookOptions, PatronOptions patronOptions, BorrowOptions borrowOptions, ReportsOptions reportsOptions)
    {
        _bookOptions = bookOptions;
        _patronOptions = patronOptions;
        _borrowOptions = borrowOptions;
        _reportsOptions = reportsOptions;
    }

    public async Task InitialMainMenu()
    {
        bool exit = false;

        while (exit == false)
        {
            AnsiConsole.Clear();
            Header.AppHeader();

            AnsiConsole.MarkupLine("[bold yellow]Menu[/]");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose a option:[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Books",
                        "2. Patrons",
                        "3. Borrow",
                        "4. Reports",
                        "5. Exit"
                    })
            );

            switch (option)
            {
                case "1. Books":
                    await _bookOptions.BookInitialOptions();
                    break;
                case "2. Patrons":
                    await _patronOptions.PatronInitialOptions();
                    break;
                case "3. Borrow":
                    await _borrowOptions.BorrowInitialOptions();
                    break;
                case "4. Reports":
                    await _reportsOptions.ReportInitialOptions();
                    break;
                case "5. Exit":
                    AnsiConsole.Clear();
                    exit = true;
                    break;
            }
        }
    }

    public async Task InitializeApp()
    {
        ShowProgressBar();
        await InitialMainMenu();
    }

    public static void ShowProgressBar()
    {
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var myTask = ctx.AddTask("[green]Initiating...[/]");
                while (!ctx.IsFinished)
                {
                    myTask.Increment(10);
                    Thread.Sleep(150);
                }
            });
    }
}