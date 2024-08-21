using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Searcher;

public class BookSearcher : ISearcher
{
    private BookRepositoryImplementation repository =
        new BookRepositoryImplementation(new BookDatasourceImplementation());
    
    public Task<IEnumerable<Book>> SearchBookByTile(string searchString)
    {
        Task<IEnumerable<Book>> books = repository.GetBooksByTitleAsync(searchString);
        return books;
    }

    public Task<IEnumerable<Book>> SearchBookByAuthor(string searchString)
    {
        Task<IEnumerable<Book>> books = repository.GetBooksByAuthorAsync(searchString);

        return books;
    }

    public Task<Book?> SearchBookByIsbn(long searchString)
    {
        var book = repository.GetBookByIsbnAsync(searchString);

        return book;
    }
    
    public IEnumerable<Patron> SearchPatronByName(string searchString)
    {
        throw new NotSupportedException("SearchPatronByName is not supported in BookSearcher.");
    }

    public Task<Patron?> SearchPatronByMembershipNumber(long searchLong)
    {
        throw new NotSupportedException("SearchPatronByMembershipNumber is not supported in BookSearcher.");
    }
}