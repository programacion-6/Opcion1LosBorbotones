using Opcion1LosBorbotones.Domain.Data;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Presentation;


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

        var app = new MainMenu(bookRepository, patronRepository, borrowRepository);
        await app.InitializeApp();
    }
}