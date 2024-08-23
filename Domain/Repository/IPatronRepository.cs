using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Domain.Repository;

public interface IPatronRepository : ICrudOperations<Patron>
{
    Task<IEnumerable<Patron>> GetPatronsByNameAsync(string name, int offset, int limit);
    Task<Patron?> GetPatronByMembershipAsync(long membership);
    Task<IEnumerable<Patron>> GetPatronsByContactDetailsAsync(long contactDetails, int offset, int limit);
}