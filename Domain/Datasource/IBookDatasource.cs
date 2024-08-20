using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Domain.Datasource;

public interface IBookDatasource : ICrudOperations<Book>
{
    Task<IEnumerable<Book>> GetBooksByTitleAsync(string title);
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
    Task<Book?> GetBookByIsbnAsync(string isbn);
    Task<IEnumerable<Book>> GetBooksByGenreAsync(BookGenre genre);
    Task<IEnumerable<Book>> GetBooksByPublicationYearAsync(DateTime publicationYear);
}