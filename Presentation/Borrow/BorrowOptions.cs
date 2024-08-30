using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Services;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Fines;
using Opcion1LosBorbotones.Presentation.Utils;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BorrowOptions
{
    public static void BorrowInitialOptions()
    {
        bool goBack = false;
        while (!goBack)
        {
            AnsiConsole.Clear();
            Header.AppHeader();
            AnsiConsole.MarkupLine("[bold yellow]Borrow Menu[/]");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Chose an option:[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "1. Request a borrow",
                        "2. Return a borrow",
                        "3. Go back"
                    })
            );
            var repository = BorrowRepositoryImplementation.GetInstance();
            var finerepository = FineRepositoryImplementation.GetInstance();

            switch (option)
            {
                case "1. Request a borrow":
                    RegisterNewBorrow(repository);
                    break;
                case "2. Return a borrow":
                    ReturnBorrow(repository, finerepository);
                    break;
                case "3. Go back":
                    goBack = true;
                    break;
            }
        }
    }

    private static void RegisterNewBorrow(BorrowRepositoryImplementation repository)
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Register a new borrow[/]");

        Guid borrowUUID = Guid.NewGuid();
        string patronId = AnsiConsole.Ask<string>("What patron id do you want to register?: ");
        Guid patronUUID = new Guid(patronId);
        string bookId = AnsiConsole.Ask<string>("What book id do you want to borrow?: ");
        Guid bookUUID = new Guid(bookId);
        BorrowStatus borrowStatus = BorrowStatus.Borrowed;
        DateTime borrowDate = DateTime.Today;
        DateTime dueDate = DateTime.Today.AddDays(15);

        AnsiConsole.MarkupLine("[bold green]Review the BORROW details before confirming:[/]");
        AnsiConsole.MarkupLine($"[bold] Borrow date [/]: {borrowDate}");
        AnsiConsole.MarkupLine($"[bold] Due date [/]: {dueDate}");
        
        var confirm = AnsiConsole.Confirm("[bold] Do you want to register this borrow? [/]");

        if (confirm)
        {
            Borrow newBorrow = new Borrow(borrowUUID, patronUUID, bookUUID, borrowStatus, dueDate, borrowDate);
            repository.CreateAsync(newBorrow);
            AnsiConsole.MarkupLine($"[bold italic green]New borrow registered:[/] {newBorrow}");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold green]No borrow registered");
        }

        AnsiConsole.Markup("[blue] Press Enter to go back to the Patron Menu.[/]");
        Console.ReadLine();
    }
    
    private static void ReturnBorrow(BorrowRepositoryImplementation borrowrepository, FineRepositoryImplementation finerepository)
    {
        AnsiConsole.Clear();
        Header.AppHeader();
        AnsiConsole.MarkupLine("[bold yellow]Return a Borrow[/]");

        Guid borrowId;
        while (true)
        {
            var borrowUUID = AnsiConsole.Ask<string>("Enter the Borrow ID:");

            if (Guid.TryParse(borrowUUID, out borrowId))
            {
                break;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid Borrow ID format.[/]");
                AnsiConsole.Markup("[blue]Please try again...[/]\n");
            }
        }

        var borrowRead = borrowrepository.ReadAsync(borrowId).GetAwaiter().GetResult();

        if (borrowRead == null)
        {
            AnsiConsole.MarkupLine("[red]Borrow record not found.[/]");
            AnsiConsole.Markup("[blue]Press Enter to continue...[/]");
            Console.ReadLine();
            return;
        }

        var updatedBorrow = new Borrow
            (
            borrowRead.Id,
            borrowRead.PatronId,
            borrowRead.BookId,
            BorrowStatus.Returned,  
            borrowRead.DueDate,
            borrowRead.BorrowDate
            );

        if (borrowRead.Status == BorrowStatus.Borrowed)
        {
            borrowrepository.UpdateAsync(updatedBorrow).GetAwaiter().GetResult();

            AnsiConsole.MarkupLine("[green]Thank you for returning the borrow on time![/]");
        }

        if (borrowRead.Status == BorrowStatus.Returned)
        {
            AnsiConsole.MarkupLine("[green]This borrow has already been returned.[/]");
        }

        if (borrowRead.Status == BorrowStatus.Reserved)
        {
            AnsiConsole.MarkupLine("[yellow]This is a reservation, no borrowing has been made yet.[/]");
        }

        if (borrowRead.Status == BorrowStatus.Overdue)
        {
            var currentDate = DateTime.Now;
            var dueDate = borrowRead.DueDate;
            var overdueDays = (currentDate - dueDate).Days;

            IFineCalculation? fineCalculation = null;
            double fineAmount = 0.0;

            if (overdueDays > 0)
            {
                if (overdueDays < 7)
                {
                    fineCalculation = new DailyFineCalculation();
                }
                else if (overdueDays < 30)
                {
                    fineCalculation = new WeeklyFineCalculation();
                }
                else if (overdueDays < 365)
                {
                    fineCalculation = new MonthlyFineCalculation();
                }
                else
                {
                    fineCalculation = new YearlyFineCalculation();
                }

                fineAmount = fineCalculation.CalculateFine(borrowRead);

                AnsiConsole.MarkupLine("[bold]Fine[/]");
                AnsiConsole.MarkupLine($"[bold]Borrow ID[/]: {borrowRead.Id}");
                AnsiConsole.MarkupLine($"[bold]Due Date[/]: {borrowRead.DueDate:yyyy-MM-dd}");
                AnsiConsole.MarkupLine($"[bold]Return Date[/]: {currentDate:yyyy-MM-dd}");
                AnsiConsole.MarkupLine($"[bold]Overdue Days[/]: {overdueDays}");
                AnsiConsole.MarkupLine($"[bold]Fine Amount[/]: ${fineAmount:F2}");

                var confirm = AnsiConsole.Confirm("[bold]Do you want to pay the fine now? [/]");

                if (confirm)
                {
                    borrowrepository.UpdateAsync(updatedBorrow).GetAwaiter().GetResult();

                    Fine newFine = new Fine(
                        Guid.NewGuid(),
                        borrowRead,
                        fineAmount,
                        true,
                        fineCalculation
                    );
                    finerepository.CreateAsync(newFine).GetAwaiter().GetResult();

                    AnsiConsole.MarkupLine("[green]Thank you! The borrow has been successfully returned and the fine has been paid.[/]");
                }
                else
                {
                    borrowrepository.UpdateAsync(updatedBorrow).GetAwaiter().GetResult();

                    var newfine = new Fine(
                        Guid.NewGuid(),
                        borrowRead,
                        fineAmount,
                        false,
                        fineCalculation
                    );
                    finerepository.CreateAsync(newfine).GetAwaiter().GetResult();

                    AnsiConsole.MarkupLine("[yellow]The fine has been recorded. Please remember to pay it later.[/]");
                }
            }
        }

        AnsiConsole.Markup("[blue]Press Enter to go back to the Borrow Menu.[/]");
        Console.ReadLine();
    }
}
