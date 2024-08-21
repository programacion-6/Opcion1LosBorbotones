using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Services.Reports;


public class Program
{
    public static async Task Main(string[] args)
    {
        BorrowStatusReport borrowStatusReport = new BorrowStatusReport();
        PatronBorrowReport patronBorrowReport = new PatronBorrowReport();
        
        Console.WriteLine("Libros prestados");
        string reportBorrowed = await borrowStatusReport.GenerateReport(BorrowStatus.Borrowed);
        Console.WriteLine(reportBorrowed); 
        
        Console.WriteLine("Libros atrasados");
        string reportOverdue = await borrowStatusReport.GenerateReport(BorrowStatus.Overdue);
        Console.WriteLine(reportOverdue); 
        
        Console.WriteLine("Libros Retornados");
        string reportReturned = await borrowStatusReport.GenerateReport(BorrowStatus.Returned);
        Console.WriteLine(reportReturned); 
        
        Console.WriteLine("Libros Reservados");
        string reportReserved = await borrowStatusReport.GenerateReport(BorrowStatus.Reserved);
        Console.WriteLine(reportReserved);
        
        Guid patronId = new Guid("a151be6c-030a-4d47-9413-d1e7f7770383");
        string reportPatronBorrowedList = await patronBorrowReport.GenerateReport(patronId);
        Console.WriteLine("Prestamos de un patron");
        Console.WriteLine(reportPatronBorrowedList);
    }
}