namespace Opcion1LosBorbotones.Domain.Repository;

public interface IBookRepository_ : IRepository<Book>
{
    Task<Book?> GetBookByISBN(long isbn);
    Task<IEnumerable<Book?>> GetBooksByTitle(string title, int offset, int limit);
    Task<IEnumerable<Book?>> GetBooksByAuthor(string author, int offset, int limit);
    Task<IEnumerable<Book?>> GetBooksByGenre(BookGenre genre, int offset, int limit);
}
