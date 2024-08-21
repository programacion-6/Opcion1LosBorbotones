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
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Report books currently borrowed[/]");
        BorrowStatusReport borrowStatusReport = new BorrowStatusReport();
        string report = borrowStatusReport.GenerateReport(BorrowStatus.Borrowed).GetAwaiter().GetResult();
        AnsiConsole.MarkupLine($"[italic]{Markup.Escape(report)}[/]");
        AnsiConsole.Markup("[blue]Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }
    
    private static void ReportBooksOverdue()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Report books overdue[/]");
        BorrowStatusReport borrowStatusReport = new BorrowStatusReport();
        string report = borrowStatusReport.GenerateReport(BorrowStatus.Overdue).GetAwaiter().GetResult();
        AnsiConsole.MarkupLine($"[italic]{Markup.Escape(report)}[/]");
        AnsiConsole.Markup("[blue]Press Enter to go back to the Report Menu.[/]");
        Console.ReadLine();
    }

    private static void ReportPatronBorrowed()
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Report parton borrowed[/]");
        
        string patronId = AnsiConsole.Ask<string>("Enter the Patron ID: ");
        Guid patronUUID = new Guid(patronId);
        PatronBorrowReport patronBorrowReport = new PatronBorrowReport();
        string report = patronBorrowReport.GenerateReport(patronUUID).GetAwaiter().GetResult();
        AnsiConsole.MarkupLine($"[italic]{Markup.Escape(report)}[/]");
        AnsiConsole.Markup("[blue]Press Enter to go back to the Report Menu.[/]");
        Console.ReadLine();
    }
}