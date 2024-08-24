using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Services.Reports;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Reports;

public class ReportsOptions
{
    public static void ReportInitialOptions()
    {
        bool goBack = false;
        while (!goBack)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Report Menu[/]");
            
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "1. Report books currently borrowed",
                        "2. Report overdue books",
                        "3. Borrowing history for a patron",
                        "4. Go back"
                    })
            );

            switch (option)
            {
                case "1. Report books currently borrowed":
                    ReportBooksCurrentlyBorrowed();
                    break;
                case "2. Report overdue books":
                    ReportBooksOverdue();
                    break;
                case "3. Borrowing history for a patron":
                    ReportPatronBorrowed();
                    break;
                case "4. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private static void ReportBooksCurrentlyBorrowed()
    {
        int offset = 0;
        const int limit = 10;
        bool exit = false;

        while (!exit)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Report books currently borrowed[/]");
            BorrowStatusReport borrowStatusReport = new BorrowStatusReport();
            string report = borrowStatusReport.GenerateReport(BorrowStatus.Borrowed, offset, limit).GetAwaiter().GetResult();
            AnsiConsole.MarkupLine($"[italic]{Markup.Escape(report)}[/]");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Choose an option:[/]")
                    .AddChoices(new[]
                    {
                        "Next Page",
                        "Previous Page",
                        "Go back to the Report Menu"
                    })
            );

            switch (option)
            {
                case "Next Page":
                    offset += limit;
                    break;
                case "Previous Page":
                    if (offset > 0)
                        offset -= limit;
                    break;
                case "Go back to the Report Menu":
                    exit = true;
                    break;
            }
        }
    }

    
    private static void ReportBooksOverdue()
    {
        int offset = 0;
        const int limit = 10;
        bool exit = false;

        while (!exit)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Report books overdue[/]");
            BorrowStatusReport borrowStatusReport = new BorrowStatusReport();
            string report = borrowStatusReport.GenerateReport(BorrowStatus.Overdue, offset, limit).GetAwaiter().GetResult();
            AnsiConsole.MarkupLine($"[italic]{Markup.Escape(report)}[/]");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Choose an option:[/]")
                    .AddChoices(new[]
                    {
                        "Next Page",
                        "Previous Page",
                        "Go back to the Report Menu"
                    })
            );

            switch (option)
            {
                case "Next Page":
                    offset += limit;
                    break;
                case "Previous Page":
                    if (offset > 0)
                        offset -= limit;
                    break;
                case "Go back to the Report Menu":
                    exit = true;
                    break;
            }
        }
    }

    private static void ReportPatronBorrowed()
    {
        int offset = 0;
        const int limit = 10;
        bool exit = false;

        while (!exit)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Report patron borrowed[/]");

            string patronId = AnsiConsole.Ask<string>("Enter the Patron ID: ");
            Guid patronUUID = new Guid(patronId);
            PatronBorrowReport patronBorrowReport = new PatronBorrowReport();
            string report = patronBorrowReport.GenerateReport(patronUUID, offset, limit).GetAwaiter().GetResult();
            AnsiConsole.MarkupLine($"[italic]{Markup.Escape(report)}[/]");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Choose an option:[/]")
                    .AddChoices(new[]
                    {
                        "Next Page",
                        "Previous Page",
                        "Go back to the Report Menu"
                    })
            );

            switch (option)
            {
                case "Next Page":
                    offset += limit;
                    break;
                case "Previous Page":
                    if (offset > 0)
                        offset -= limit;
                    break;
                case "Go back to the Report Menu":
                    exit = true;
                    break;
            }
        }
    }
}