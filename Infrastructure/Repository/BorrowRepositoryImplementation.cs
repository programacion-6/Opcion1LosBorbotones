using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Datasource;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class BorrowRepositoryImplementation : IBorrowRepository
{
    private readonly IBorrowDatasource _dataSource;
    private static BorrowRepositoryImplementation _instance;

    public BorrowRepositoryImplementation(IBorrowDatasource dataSource)
    {
        _dataSource = dataSource;
    }
    
    public static BorrowRepositoryImplementation GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BorrowRepositoryImplementation(new BorrowDatasourceImplementation());
        }
        return _instance;
    }
    
    public async Task<Borrow> CreateAsync(Borrow entity)
    {
        return await _dataSource.CreateAsync(entity);
    }

    public async Task<Borrow?> ReadAsync(Guid id)
    {
        return await _dataSource.ReadAsync(id);
    }

    public async Task<Borrow> UpdateAsync(Borrow entity)
    {
        return await _dataSource.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _dataSource.DeleteAsync(id);
    }

    public async Task<IEnumerable<Borrow>> GetAllAsync()
    {
        return await _dataSource.GetAllAsync();
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByPatron(Guid patronId)
    {
        return await _dataSource.GetBorrowsByPatron(patronId);
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByBook(Guid bookId)
    {
        return await _dataSource.GetBorrowsByBook(bookId);
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByStatus(BorrowStatus status)
    {
        return await _dataSource.GetBorrowsByStatus(status);
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByDueDate(DateTime dueDate)
    {
        return await _dataSource.GetBorrowsByDueDate(dueDate);
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByBorrowDate(DateTime borrowDate)
    {
        return await _dataSource.GetBorrowsByBorrowDate(borrowDate);
    }
}