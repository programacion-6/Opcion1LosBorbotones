using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Services.Searcher;

public class Program
{
    public static async Task Main(string[] args)
    {
        var PatronSearcher = new PatronSearcher();
        IEnumerable<Patron> patrons = PatronSearcher.SearchPatronByName("Ethan Thomas"); //CHANGUE THE NAME
        Console.WriteLine("Patrons segun el nombre:"); 
        foreach (var patron in patrons)
        {
            Console.WriteLine($"ID: {patron.Id}, Nombre: {patron.Name}, Membresía: {patron.MembershipNumber}");
        }

        Task<Patron?> patorn = PatronSearcher.SearchPatronByMembershipNumber(1759764659);
        Console.WriteLine("Patron segun membershipNumber: ");
        Console.WriteLine(patorn?.Result);
        
        var BookSearcher = new BookSearcher();
        Task<IEnumerable<Book>> booksTaskTitle = BookSearcher.SearchBookByTile("The Grapes of Wrath");
        IEnumerable<Book> booksByTitle = await booksTaskTitle;
        
        Console.WriteLine("Books segun el titulo:");
        foreach (var book in booksByTitle)
        {
            Console.WriteLine(book.ToString());
        }
        
        Task<IEnumerable<Book>> booksTaskAuthor = BookSearcher.SearchBookByAuthor("Harper Lee");
        IEnumerable<Book> booksByAuthor = await booksTaskAuthor;
        
        Console.WriteLine("Books segun el autor:");
        foreach (var book in booksByAuthor)
        {
            Console.WriteLine(book.ToString());
        }
        
        Task<Book?> bookByIsbn = BookSearcher.SearchBookByIsbn(3206890312313);
        Console.WriteLine("Books segun el ISBN:");
        Console.WriteLine(bookByIsbn?.Result);
    }
}