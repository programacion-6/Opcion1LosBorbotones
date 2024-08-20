using Opcion1LosBorbotones.Domain.Services;
namespace Opcion1LosBorbotones.Domain.Repository;

public interface IBookRepository : ICrudOperations<Book>
{
    Task<IEnumerable<Book>> GetBookByTitleAsync(string title);
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
    Task<Book?> GetBookByIsbnAsync(string isbn);
    Task<IEnumerable<Book>> GetBooksByGenreAsync(BookGenre genre);
    Task<IEnumerable<Book>> GetBooksByPublicationYearAsync(DateTime publicationYear);
}