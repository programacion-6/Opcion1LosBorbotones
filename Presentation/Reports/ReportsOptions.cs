using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Reports;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Reports;

public class ReportsOptions
{
    private readonly BorrowStatusReport _borrowStatusReport;
    private readonly PatronBorrowReport _patronBorrowReport;

    public ReportsOptions(IBorrowRepository borrowRepository)
    {
        _borrowStatusReport = new BorrowStatusReport(borrowRepository);
        _patronBorrowReport = new PatronBorrowReport(borrowRepository);
    }

    public async Task ReportInitialOptions()
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
                    await ReportBooksCurrentlyBorrowed();
                    break;
                case "2. Report overdue books":
                    await ReportBooksOverdue();
                    break;
                case "3. Borrowing history for a patron":
                    await ReportPatronBorrowed();
                    break;
                case "4. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private async Task ReportBooksCurrentlyBorrowed()
    {
        int offset = 0;
        const int limit = 10;
        bool exit = false;

        while (!exit)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Report books currently borrowed[/]");
            string report = await _borrowStatusReport.GenerateReport(BorrowStatus.Borrowed, offset, limit);
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

    
    private async Task ReportBooksOverdue()
    {
        int offset = 0;
        const int limit = 10;
        bool exit = false;

        while (!exit)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Report books overdue[/]");
            string report = await _borrowStatusReport.GenerateReport(BorrowStatus.Overdue, offset, limit);
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

    private async Task ReportPatronBorrowed()
    {
        int offset = 0;
        const int limit = 10;
        bool exit = false;

        while (!exit)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Report patron borrowed[/]");

            long membershipNumber = AnsiConsole.Ask<long>("Enter the Patron MembershipNumber: ");
            long patronMembershipNumber = membershipNumber;
            string report = await _patronBorrowReport.GenerateReport(patronMembershipNumber, offset, limit);
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