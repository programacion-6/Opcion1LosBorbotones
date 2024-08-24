using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Domain.Datasource;

public interface IBorrowDatasource : ICrudOperations<Borrow>
{
    Task<IEnumerable<Borrow>> GetBorrowsByPatron(Guid patronId, int offset, int limit);
    Task<IEnumerable<Borrow>> GetBorrowsByBook(Guid bookId, int offset, int limit);
    Task<IEnumerable<Borrow>> GetBorrowsByStatus(BorrowStatus status, int offset, int limit);
    Task<IEnumerable<Borrow>> GetBorrowsByDueDate(DateTime dueDate, int offset, int limit);
    Task<IEnumerable<Borrow>> GetBorrowsByBorrowDate(DateTime borrowDate, int offset, int limit);
}