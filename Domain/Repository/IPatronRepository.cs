using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain.Repository;

public interface IPatronRepository : IRepository<Patron>
{
    Task<IEnumerable<Patron>> GetPatronsByNameAsync(string name, int offset, int limit);
    Task<Patron?> GetPatronByMembershipAsync(long membership);
    Task<IEnumerable<Patron?>> GetPatronsByContactDetailsAsync(long contactDetails, int offset, int limit);
}
