using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Domain.Datasource;

public interface IFineDatasource : ICrudOperations<Fine>
{
    Task<IEnumerable<Fine>> GetFinesByBorrowIdAsync(Guid borrowId);
    Task<IEnumerable<Fine>> GetFinesByStatusAsync(bool isPaid);
}
