using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Searcher;

public class BookSearcher : ISearcher
{
    private BookRepositoryImplementation repository = BookRepositoryImplementation.GetInstance();
    
    public Task<IEnumerable<Book>> SearchBookByTile(string searchString, int offset, int limit)
    {
        Task<IEnumerable<Book>> books = repository.GetBooksByTitleAsync(searchString, offset, limit);
        return books;
    }

    public Task<IEnumerable<Book>> SearchBookByAuthor(string searchString, int offset, int limit)
    {
        Task<IEnumerable<Book>> books = repository.GetBooksByAuthorAsync(searchString, offset, limit);

        return books;
    }

    public Task<Book?> SearchBookByIsbn(long searchLong)
    {
        var book = repository.GetBookByIsbnAsync(searchLong);

        return book;
    }
    
    public IEnumerable<Patron> SearchPatronByName(string searchString, int offset, int limit)
    {
        throw new NotSupportedException("SearchPatronByName is not supported in BookSearcher.");
    }

    public Task<Patron?> SearchPatronByMembershipNumber(long searchLong)
    {
        throw new NotSupportedException("SearchPatronByMembershipNumber is not supported in BookSearcher.");
    }
}