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
        ReportBooksBorrowed reportBooksBorrowed = new ReportBooksBorrowed();
        
        Console.WriteLine("Libros prestados");
        string reportBorrowed = await reportBooksBorrowed.GenerateReport(BorrowStatus.Borrowed);
        
        Console.WriteLine(reportBorrowed); 
        
        Console.WriteLine("Libros atrasados");
        string reportOverdue = await reportBooksBorrowed.GenerateReport(BorrowStatus.Overdue);
        
        Console.WriteLine(reportOverdue); 
        
        Console.WriteLine("Libros Retornados");
        string reportrReturned = await reportBooksBorrowed.GenerateReport(BorrowStatus.Returned);
        
        Console.WriteLine(reportrReturned); 
        
        Console.WriteLine("Libros Reservados");
        string reportrReservados = await reportBooksBorrowed.GenerateReport(BorrowStatus.Reserved);
        
        Console.WriteLine(reportrReservados); 
    }
}