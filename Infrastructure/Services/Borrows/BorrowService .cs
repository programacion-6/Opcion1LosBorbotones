using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Borrows;

public class BorrowService : IBorrowService
{
    private readonly IBorrowRepository _borrowRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;

    public BorrowService(IBorrowRepository borrowRepository, IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _borrowRepository = borrowRepository;
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public async Task<Guid> GetBookIdByISBN(long isbn)
    {
        var book = await _bookRepository.GetBookByISBN(isbn);
        return book?.Id ?? Guid.Empty;
    }

    public async Task<Guid> GetPatronIdByMembershipNumber(long membershipNumber)
    {
        var patron = await _patronRepository.GetPatronByMembershipAsync(membershipNumber);
        return patron?.Id ?? Guid.Empty;
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