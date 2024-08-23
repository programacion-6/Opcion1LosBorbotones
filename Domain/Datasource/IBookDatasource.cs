using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Domain.Datasource;

public interface IBookDatasource : ICrudOperations<Book>
{
    Task<Book?> GetBookByIsbnAsync(long isbn);
    Task<IEnumerable<Book>> GetBooksByTitleAsync(string title, int offset, int limit);
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author, int offset, int limit);
    Task<IEnumerable<Book>> GetBooksByGenreAsync(BookGenre genre, int offset, int limit);
    Task<IEnumerable<Book>> GetBooksByPublicationYearAsync(DateTime publicationYear, int offset, int limit);
}
