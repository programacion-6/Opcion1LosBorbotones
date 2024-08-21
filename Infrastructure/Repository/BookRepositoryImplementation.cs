using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class BookRepositoryImplementation : IBookRepository
{
    private readonly IBookDatasource _dataSource;

    public BookRepositoryImplementation(IBookDatasource dataSource)
    {
        this._dataSource = dataSource;
    }

    public async Task<Book> CreateAsync(Book entity)
    {
        return await _dataSource.CreateAsync(entity);
    }

    public async Task<Book?> ReadAsync(Guid id)
    {
        return await _dataSource.ReadAsync(id);
    }

    public async Task<Book> UpdateAsync(Book entity)
    {
        return await _dataSource.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _dataSource.DeleteAsync(id);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _dataSource.GetAllAsync();;
    }

    public async Task<IEnumerable<Book>> GetBooksByTitleAsync(string title)
    {
        return await _dataSource.GetBooksByTitleAsync(title);
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
    {
        return await _dataSource.GetBooksByAuthorAsync(author);
    }

    public async Task<Book?> GetBookByIsbnAsync(long isbn)
    {
        return await _dataSource.GetBookByIsbnAsync(isbn);
    }

    public async Task<IEnumerable<Book>> GetBooksByGenreAsync(BookGenre genre)
    {
        return await _dataSource.GetBooksByGenreAsync(genre);
    }

    public async Task<IEnumerable<Book>> GetBooksByPublicationYearAsync(DateTime publicationYear)
    {
        return await _dataSource.GetBooksByPublicationYearAsync(publicationYear);
    }
}