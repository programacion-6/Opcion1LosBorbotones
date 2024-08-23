using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;


public class Program
{
    public static async Task Main(string[] args)
    {
        var repository = new BookRepositoryImplementation(new BookDatasourceImplementation());
        
        Console.WriteLine("PRIMEROS 3");
        //primeros 3
        IEnumerable<Book> books = await repository.GetAllAsync(0,3);
        foreach (var book in books)
        {
            Console.WriteLine(book.ToString());
            
        }
        
        Console.WriteLine("SIGUIENTES 3");
        //siguientes 3
        books = await repository.GetAllAsync(3,3);
        foreach (var book in books)
        {
            Console.WriteLine(book.ToString());
            
        }
        
        Console.WriteLine("PRIMEROS 3 DE GENERO HORROR");
        books = await repository.GetBooksByGenreAsync(BookGenre.Horror,0,3);
        foreach (var book in books)
        {
            Console.WriteLine(book.ToString());
            
        }
        
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("TEST BORROWS");
        Console.WriteLine("---------------------------------------");
        var borrowRepository = new BorrowRepositoryImplementation(new BorrowDatasourceImplementation());
        
        Console.WriteLine("PRIMEROS 3");
        //primeros 3
        IEnumerable<Borrow> borrows = await borrowRepository.GetAllAsync(0,3);
        foreach (var book in borrows)
        {
            Console.WriteLine(book.ToString());
            
        }
        
        Console.WriteLine("SIGUIENTES 3");
        //siguientes 3
        borrows = await borrowRepository.GetAllAsync(3,3);
        foreach (var book in borrows)
        {
            Console.WriteLine(book.ToString());
            
        }
        
        Console.WriteLine("PRIMEROS 3 DE PRESTADOS");
        borrows = await borrowRepository.GetBorrowsByStatus(BorrowStatus.Borrowed,0,3);
        foreach (var book in borrows)
        {
            Console.WriteLine(book.ToString());
            
        }
        
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("TEST PATRONS");
        Console.WriteLine("---------------------------------------");
        var patronRepository = new PatronRepositoryImplementation(new PatronDatasourceImplementation());
        
        Console.WriteLine("PRIMEROS 3");
        //primeros 3
        IEnumerable<Patron> patrons = await patronRepository.GetAllAsync(0,3);
        foreach (var patron in patrons)
        {
            Console.WriteLine(patron.ToString());
            
        }
        
        Console.WriteLine("SIGUIENTES 3");
        //siguientes 3
        patrons = await patronRepository.GetAllAsync(3,3);
        foreach (var patron in patrons)
        {
            Console.WriteLine(patron.ToString());
            
        }
        
        Console.WriteLine("PRIMEROS 3 PATRONS, SEGUN EL NOMBRE");
        patrons = await patronRepository.GetPatronsByNameAsync("James Wilson",0,3);
        foreach (var patron in patrons)
        {
            Console.WriteLine(patron.ToString());
            
        }
        
        Console.WriteLine("SEGUN EL ID");
        Patron searchPatron = await patronRepository.GetPatronByMembershipAsync(1064986412);
        Console.WriteLine(searchPatron.ToString());
    }
}
