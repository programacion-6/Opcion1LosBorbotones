using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

public class Program
{
    public static async Task Main(string[] args)
    {
        var repository = new BookRepositoryImplementation(new BookDatasourceImplementation());
        IEnumerable<Book> books = await repository.GetBooksByTitleAsync("To Kill a Mockingbird");
        Console.WriteLine("Libros segun el titulo"); 
        foreach (var item in books)
        {
            Console.WriteLine(item); 
        }
        
        var book = await repository.ReadAsync(new Guid("edbfaefd-ebdf-432d-987f-b767b975e4da"));
        Console.WriteLine("libro segun el index: " + book);
    }
}