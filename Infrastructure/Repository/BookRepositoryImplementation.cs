using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Datasource;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class BookRepositoryImplementation : IBookRepository
{
    private readonly IBookDatasource _dataSource;
    private static BookRepositoryImplementation? _instance;

    public BookRepositoryImplementation(IBookDatasource dataSource)
    {
        this._dataSource = dataSource;
    }

    public static BookRepositoryImplementation GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BookRepositoryImplementation(new BookDatasourceImplementation());
        }

        return _instance;
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

    public async Task<IEnumerable<Book>> GetAllAsync(int offset, int limit)
    {
        return await _dataSource.GetAllAsync(offset, limit);
    }

    public async Task<IEnumerable<Book>> GetBooksByTitleAsync(string title, int offset, int limit)
    {
        return await _dataSource.GetBooksByTitleAsync(title, offset, limit);
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author, int offset, int limit)
    {
        return await _dataSource.GetBooksByAuthorAsync(author, offset, limit);
    }

    public async Task<Book?> GetBookByIsbnAsync(long isbn)
    {
        return await _dataSource.GetBookByIsbnAsync(isbn);
    }

    public async Task<IEnumerable<Book>> GetBooksByGenreAsync(BookGenre genre, int offset, int limit)
    {
        return await _dataSource.GetBooksByGenreAsync(genre, offset, limit);
    }

    public async Task<IEnumerable<Book>> GetBooksByPublicationYearAsync(DateTime publicationYear, int offset, int limit)
    {
        return await _dataSource.GetBooksByPublicationYearAsync(publicationYear, offset, limit);
    }
}