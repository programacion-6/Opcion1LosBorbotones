using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Presentation.Reports;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class MainMenu
{
    public static void InitialMainMenu()
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
                     BookOptions.BookInitialOptions();
                    break;
                case "2. Patrons":
                    PatronOptions.PatronInitialOptions();
                    break;
                case "3. Borrow":
                    BorrowOptions.BorrowInitialOptions();
                    break;
                case "4. Reports":
                    ReportsOptions.ReportInitialOptions();
                    break;
                case "5. Exit":
                    AnsiConsole.Clear();
                    exit = true;
                    break;
            }
        }
    }

    public static void InitializeApp()
    {
        ShowProgressBar();
        InitialMainMenu();
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