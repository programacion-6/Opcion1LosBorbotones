using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Domain.Repository;

public interface IBorrowRepository : ICrudOperations<Borrow>
{
    Task<IEnumerable<Borrow>> GetBorrowsByPatron(Guid patronId);
    Task<IEnumerable<Borrow>> GetBorrowsByBook(Guid bookId);
    Task<IEnumerable<Borrow>> GetBorrowsByStatus(BorrowStatus status);
    Task<IEnumerable<Borrow>> GetBorrowsByDueDate(DateTime dueDate);
    Task<IEnumerable<Borrow>> GetBorrowsByBorrowDate(DateTime borrowDate);
}