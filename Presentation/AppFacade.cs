using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Data;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Borrows;
using Opcion1LosBorbotones.Presentation.Executors;
using Opcion1LosBorbotones.Presentation.Handlers;
using Opcion1LosBorbotones.Presentation.Renders;

namespace Opcion1LosBorbotones.Presentation;

public class ApplicationFacade
{
    private readonly DatabaseConfig _databaseConfig;

    public ApplicationFacade()
    {
        _databaseConfig = new DatabaseConfig();
    }

    public async Task<MainHandlerExecutor> CreateAppAsync()
    {
        await InitDatabaseConnection();
        IBookRepository bookRepository = new BookRepository(_databaseConfig.ConnectionString);
        IPatronRepository patronRepository = new PatronRepository(_databaseConfig.ConnectionString);
        IBorrowRepository borrowRepository = new BorrowRepository(_databaseConfig.ConnectionString);
        var mainExecutor = CreateMainExecutor(bookRepository, patronRepository, borrowRepository);

        return mainExecutor;
    }

    private async Task InitDatabaseConnection()
    {
        try
        {
            using var connection = _databaseConfig.CreateConnection();
            await connection.OpenAsync();
        }
        catch (Exception ex)
        {
            ConsoleMessageRenderer.RenderErrorMessage("Could not connect to the database. Please verify that it is running.");
            ConsoleMessageRenderer.RenderErrorMessage(ex.Message);
            Environment.Exit(1);
        }
    }

    private MainHandlerExecutor CreateMainExecutor(IBookRepository _bookRepository, IPatronRepository _patronRepository, IBorrowRepository _borrowRepository)
    {
        IEntityRequester<Book> bookRequester = new BookRequesterByConsole();
        IEntityRequester<Patron> patronRequester = new PatronRequesterByConsole();

        var borrowService = new BorrowService(_borrowRepository, _bookRepository, _patronRepository);
        var bookFinder = new BookFinderExecutor(_bookRepository);
        var patronFinder = new PatronFinderExecutor(_patronRepository);

        IEntityFormatterFactory<Borrow> _formatterFactoryBorrow = new BorrowFormatterFactory(_bookRepository, _patronRepository);
        //IEntityFormatterFactory<Patron> _formatterFactoryPatron = new PatronFormatterFactory();
        
        var bookOptions = new BookHandlerExecutor(_bookRepository, bookRequester, bookFinder);
        var patronOptions = new PatronHandlerExecutor(_patronRepository, patronRequester, patronFinder);
        var borrowOptions = new LoanHandlerExecutor(borrowService, _formatterFactoryBorrow, _patronRepository, _bookRepository);
        var reportOptions = new ReportHandlerExecutor(_borrowRepository);

        return new MainHandlerExecutor(bookOptions, patronOptions, borrowOptions, reportOptions);
    }
}
