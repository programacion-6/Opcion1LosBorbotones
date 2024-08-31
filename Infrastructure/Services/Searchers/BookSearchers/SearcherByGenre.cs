using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Searchers.BookSearchers;

public class SearcherByGenre : ISearchStrategy<Book, string>
{
    private IBookRepository _bookRepository;

    public SearcherByGenre(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> SearchByPage(string criteria, int pageNumber, int pageSize)
    {
        var books = await _bookRepository.GetBooksByGenre(criteria, pageNumber, pageSize);
        return books.ToList();
    }
}