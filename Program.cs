using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

public class Program
{
    public static async Task Main(string[] args)
    {
        var repository = new BookRepositoryImplementation(new BookDatasourceImplementation());
        IEnumerable<Book> books = await repository.GetBooksByTitleAsync("To Kill a Mockingbird"); //CHANGUE THE TITLE
        Console.WriteLine("Libros segun el titulo:"); 
        foreach (var item in books)
        {
            Console.WriteLine(item); 
        }
        
        var book = await repository.ReadAsync(new Guid("83d70e67-dd89-4979-9888-ece2b91bc27c")); //CHANGE THE GUID 
        Console.WriteLine("libro segun el id: " + book);
        
        book = await repository.GetBookByIsbnAsync(2409886367626); //CHANGE THE ISBN 
        Console.WriteLine("libro segun el isbn: " + book);
    }
}