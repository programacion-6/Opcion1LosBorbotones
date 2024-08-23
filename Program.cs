using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;


public class Program
{
    public static async Task Main(string[] args)
    {
        var repository = new BookRepositoryImplementation(new BookDatasourceImplementation());
        
        Console.WriteLine("PRIMEROS 3");
        //primeros 3
        IEnumerable<Book> allBorrows = await repository.GetAllAsync(0,3);
        foreach (var book in allBorrows)
        {
            Console.WriteLine(book.ToString());
            
        }
        
        Console.WriteLine("SIGUIENTES 3");
        //siguientes 3
        allBorrows = await repository.GetAllAsync(3,3);
        foreach (var book in allBorrows)
        {
            Console.WriteLine(book.ToString());
            
        }
        
        Console.WriteLine("PRIMEROS 3 DE GENERO HORROR");
        allBorrows = await repository.GetBooksByGenreAsync(BookGenre.Horror,0,3);
        foreach (var book in allBorrows)
        {
            Console.WriteLine(book.ToString());
            
        }
    }
}
