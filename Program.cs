using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Searcher;

public class Program
{
    public static async Task Main(string[] args)
    {
        var repository = new BorrowRepositoryImplementation(new BorrowDatasourceImplementation());
        var patronRepository = new PatronRepositoryImplementation(new PatronDatasourceImplementation());
        var bookRepository = new BookRepositoryImplementation(new BookDatasourceImplementation());
        IEnumerable<Borrow> borrows = await repository.GetBorrowsByStatus(BorrowStatus.Borrowed); //CHANGUE THE STATUS
        Console.WriteLine("Borrows segun el status:"); 
        foreach (var item in borrows)
        {
            Console.WriteLine(item); 
        }

        

        IEnumerable<Borrow> allBorrows = await repository.GetAllAsync();
        foreach (var borrow in allBorrows)
        {
            var patron = await patronRepository.ReadAsync(borrow.PatronId);
            var book = await bookRepository.ReadAsync(borrow.BookId);
            Console.WriteLine("El patron: " + patron.Name + " hizo el prestamo del libro:  " + book.Title + " y el estado es: " + borrow.Status);
        }
    }
}