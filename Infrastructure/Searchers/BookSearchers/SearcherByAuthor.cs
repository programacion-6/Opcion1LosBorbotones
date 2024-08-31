using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Searchers;

public class SearcherByAuthor : ISearchStrategy<Book, string>
{
    private IBookRepository _bookRepository;

    public SearcherByAuthor(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public string GetPrompt()
    {
        return "Book author";
    }

    public async Task<List<Book>> SearchByPage(string criteria, int pageNumber, int pageSize)
    {
        var books = await _bookRepository.GetBooksByAuthor(criteria, pageNumber, pageSize);
        return books.ToList();
    }
}