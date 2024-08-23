using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Datasource;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class PatronRepositoryImplementation : IPatronRepository
{
    private readonly IPatronDatasource _dataSource;
    private static PatronRepositoryImplementation _instance;

    public PatronRepositoryImplementation(IPatronDatasource dataSource)
    {
        _dataSource = dataSource;
    }
    
    public static PatronRepositoryImplementation GetInstance()
    {
        if (_instance == null)
        {
            _instance = new PatronRepositoryImplementation(new PatronDatasourceImplementation());
        }

        return _instance;
    }
    
    public async Task<Patron> CreateAsync(Patron entity)
    {
        return await _dataSource.CreateAsync(entity);
    }

    public async Task<Patron?> ReadAsync(Guid id)
    {
        return await _dataSource.ReadAsync(id);
    }

    public async Task<Patron> UpdateAsync(Patron entity)
    {
        return await _dataSource.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _dataSource.DeleteAsync(id);
    }

    public async Task<IEnumerable<Patron>> GetAllAsync(int offset, int limit)
    {
        return await _dataSource.GetAllAsync(offset, limit);

    }

    public async Task<IEnumerable<Patron>> GetPatronsByNameAsync(string name, int offset, int limit)
    {
        return await _dataSource.GetPatronsByNameAsync(name, offset, limit);
    }

    public async Task<Patron?> GetPatronByMembershipAsync(long membership)
    {
        return await _dataSource.GetPatronByMembershipAsync(membership);
    }

    public async Task<IEnumerable<Patron>> GetPatronsByContactDetailsAsync(long contactDetails, int offset, int limit)
    {
        return await _dataSource.GetPatronsByContactDetailsAsync(contactDetails, offset, limit);
    }
}