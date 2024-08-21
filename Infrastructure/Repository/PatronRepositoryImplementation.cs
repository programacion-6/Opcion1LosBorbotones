using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class PatronRepositoryImplementation : IPatronRepository
{
    private readonly IPatronDatasource _dataSource;

    public PatronRepositoryImplementation(IPatronDatasource dataSource)
    {
        _dataSource = dataSource;
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

    public async Task<IEnumerable<Patron>> GetAllAsync()
    {
        return await _dataSource.GetAllAsync();

    }

    public async Task<IEnumerable<Patron>> GetPatronsByNameAsync(string name)
    {
        return await _dataSource.GetPatronsByNameAsync(name);
    }

    public async Task<Patron?> GetPatronByMembershipAsync(long membership)
    {
        return await _dataSource.GetPatronByMembershipAsync(membership);
    }

    public async Task<IEnumerable<Patron>> GetPatronsByContactDetailsAsync(long contactDetails)
    {
        return await _dataSource.GetPatronsByContactDetailsAsync(contactDetails);
    }
}