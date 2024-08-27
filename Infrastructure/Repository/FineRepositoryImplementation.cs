using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Infrastructure.Datasource;


namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class FineRepositoryImplementation : IFineRepository
{
    private readonly IFineDatasource _dataSource;
    private static FineRepositoryImplementation? _instance;

    public FineRepositoryImplementation(IFineDatasource dataSource)
    {
        _dataSource = dataSource;
    }

    public static FineRepositoryImplementation GetInstance()
    {
        if (_instance == null)
        {
            _instance = new FineRepositoryImplementation(new FineDatasourceImplementation());
        }
        return _instance;
    }

    public async Task<Fine> CreateAsync(Fine entity)
    {
        return await _dataSource.CreateAsync(entity);
    }

    public async Task<Fine?> ReadAsync(Guid id)
    {
        return await _dataSource.ReadAsync(id);
    }

    public async Task<Fine> UpdateAsync(Fine entity)
    {
        return await _dataSource.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _dataSource.DeleteAsync(id);
    }

    public async Task<IEnumerable<Fine>> GetAllAsync(int offset, int limit)
    {
        return await _dataSource.GetAllAsync(offset, limit);
    }

    public async Task<IEnumerable<Fine>> GetFinesByBorrowIdAsync(Guid borrowId)
    {
        return await _dataSource.GetFinesByBorrowIdAsync(borrowId);
    }

    public async Task<IEnumerable<Fine>> GetFinesByStatusAsync(bool isPaid)
    {
        return await _dataSource.GetFinesByStatusAsync(isPaid);
    }
}
