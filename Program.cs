using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Presentation;


public class Program
{
    public static async Task Main(string[] args)
    {
        IBookRepository bookRepository = new BookRepository();
        IPatronRepository patronRepository = new PatronRepository();

        var app = new MainMenu(bookRepository, patronRepository);
        await app.InitializeApp();
    }
}