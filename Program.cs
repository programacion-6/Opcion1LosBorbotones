using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Reports;
using Opcion1LosBorbotones.Infrastructure.Services.Searcher;

public class Program
{
    public static async Task Main(string[] args)
    {
        ReportBooksStatus reportBooksStatus = new ReportBooksStatus();
        ReportPatronBorrowed reportPatronBorrowed = new ReportPatronBorrowed();
        
        Console.WriteLine("Libros prestados");
        string reportBorrowed = await reportBooksStatus.GenerateBorrowStatusReport(BorrowStatus.Borrowed);
        
        Console.WriteLine(reportBorrowed); 
        
        Console.WriteLine("Libros atrasados");
        string reportOverdue = await reportBooksStatus.GenerateBorrowStatusReport(BorrowStatus.Overdue);
        
        Console.WriteLine(reportOverdue); 
        
        Console.WriteLine("Libros Retornados");
        string reportrReturned = await reportBooksStatus.GenerateBorrowStatusReport(BorrowStatus.Returned);
        
        Console.WriteLine(reportrReturned); 
        
        Console.WriteLine("Libros Reservados");
        string reportrReservados = await reportBooksStatus.GenerateBorrowStatusReport(BorrowStatus.Reserved);
        
        Console.WriteLine(reportrReservados);
        
        Guid patronId = new Guid("a151be6c-030a-4d47-9413-d1e7f7770383");
        string reportPatronBorrowedList = await reportPatronBorrowed.GeneratePatronBorrowReport(patronId);
        Console.WriteLine("Prestamos de un patron");
        
        Console.WriteLine(reportPatronBorrowedList);
    }
}