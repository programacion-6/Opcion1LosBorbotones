using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class BookRepositoryImplementation : IBookRepository
{
    private readonly IBookDatasource dataSource;

    public BookRepositoryImplementation(IBookDatasource dataSource)
    {
        this.dataSource = dataSource;
    }

    public async Task<Book> CreateAsync(Book entity)
    {
        return await dataSource.CreateAsync(entity);
    }

    public async Task<Book?> ReadAsync(Guid id)
    {
        return await dataSource.ReadAsync(id);
    }

    public async Task<Book> UpdateAsync(Book entity)
    {
        return await dataSource.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await dataSource.DeleteAsync(id);
    }

    public async Task<IEnumerable<Book>> GetBooksByTitleAsync(string title)
    {
        return await dataSource.GetBooksByTitleAsync(title);
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
    {
        return await dataSource.GetBooksByAuthorAsync(author);
    }

    public async Task<Book?> GetBookByIsbnAsync(long isbn)
    {
        return await dataSource.GetBookByIsbnAsync(isbn);
    }

    public async Task<IEnumerable<Book>> GetBooksByGenreAsync(BookGenre genre)
    {
        return await dataSource.GetBooksByGenreAsync(genre);
    }

    public async Task<IEnumerable<Book>> GetBooksByPublicationYearAsync(DateTime publicationYear)
    {
        return await dataSource.GetBooksByPublicationYearAsync(publicationYear);
    }
}