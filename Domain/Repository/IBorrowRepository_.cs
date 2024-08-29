using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain.Repository;

public interface IBorrowRepository_ : IRepository<Borrow>
{
    Task<IEnumerable<Borrow?>> GetBorrowsByPatron(Guid patronId, int offset, int limit);
    Task<IEnumerable<Borrow?>> GetBorrowsByBook(Guid bookId, int offset, int limit);
    Task<IEnumerable<Borrow?>> GetBorrowsByStatus(BorrowStatus status, int offset, int limit);
    Task<IEnumerable<Borrow?>> GetBorrowsByDueDate(DateTime dueDate, int offset, int limit);
    Task<IEnumerable<Borrow?>> GetBorrowsByBorrowDate(DateTime borrowDate, int offset, int limit);
}
