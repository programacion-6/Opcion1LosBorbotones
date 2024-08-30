using LibrarySystem;
using Opcion1LosBorbotones;
using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Data;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Borrows;
using Opcion1LosBorbotones.Presentation;
using Opcion1LosBorbotones.Presentation.Reports;


public class Program
{
    public static async Task Main(string[] args)
    {
        var databaseConfig = new DatabaseConfig();
        using var connection = databaseConfig.CreateConnection();

        await connection.OpenAsync();

        IBookRepository bookRepository = new BookRepository(databaseConfig.ConnectionString);
        IPatronRepository patronRepository = new PatronRepository(databaseConfig.ConnectionString);
        IBorrowRepository borrowRepository = new BorrowRepository(databaseConfig.ConnectionString);

        IEntityRequester<Book> bookRequester = new BookRequesterByConsole();
        IEntityRequester<Patron> patronRequester = new PatronRequesterByConsole();

        var borrowService = new BorrowService(borrowRepository);

        var bookOptions = new BookOptions(bookRepository, bookRequester);
        var patronOptions = new PatronOptions(patronRepository, patronRequester);
        var borrowOptions = new BorrowOptions(borrowService);
        var reportOptions = new ReportsOptions(borrowRepository);

        var app = new MainMenu(bookOptions, patronOptions, borrowOptions, reportOptions);
        await app.InitializeApp();
    }
}