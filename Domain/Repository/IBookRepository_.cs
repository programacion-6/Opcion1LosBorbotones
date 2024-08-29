namespace Opcion1LosBorbotones.Domain.Repository;

public interface IBookRepository_ : IRepository<Book>
{
    Task<Book?> GetBookByISBN(long isbn);
    Task<IEnumerable<Book?>> GetBooksByTitle(string title);
    Task<IEnumerable<Book?>> GetBooksByAuthor(string author);
    Task<IEnumerable<Book?>> GetBooksByGenre(BookGenre genre);
}
