using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Borrows;

public class BorrowService : IBorrowService
{
    private readonly IBorrowRepository _borrowRepository;

    public BorrowService(IBorrowRepository borrowRepository)
    {
        _borrowRepository = borrowRepository;
    }

    public async Task<Borrow> RegisterNewBorrow(Guid patronUUID, Guid bookUUID)
    {
        Guid borrowUUID = Guid.NewGuid();
        BorrowStatus borrowStatus = BorrowStatus.Borrowed;
        DateTime borrowDate = DateTime.Today;
        DateTime dueDate = DateTime.Today.AddDays(15);

        Borrow newBorrow = new Borrow(borrowUUID, patronUUID, bookUUID, borrowStatus, dueDate, borrowDate);
        await _borrowRepository.Save(newBorrow);

        return newBorrow;
    }
}