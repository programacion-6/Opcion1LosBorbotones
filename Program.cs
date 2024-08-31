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

        var borrowService = new BorrowService(borrowRepository, bookRepository, patronRepository);

        IEntityFormatterFactory<Borrow> _formatterFactoryBorrow = new BorrowFormatterFactory(bookRepository, patronRepository);
        IEntityFormatterFactory<Patron> _formatterFactoryPatron = new PatronFormatterFactory();
        IEntityFormatterFactory<Book> _formatterFactoryBook = new BookFormatterFactory();
        
        var bookOptions = new BookOptions(bookRepository, bookRequester, _formatterFactoryBook);
        var patronOptions = new PatronOptions(patronRepository, patronRequester, _formatterFactoryPatron);
        var borrowOptions = new BorrowOptions(borrowService, _formatterFactoryBorrow, patronRepository, bookRepository);
        var reportOptions = new ReportsOptions(borrowRepository, _formatterFactoryBorrow);

        var app = new MainMenu(bookOptions, patronOptions, borrowOptions, reportOptions);
        await app.InitializeApp();
    }
}