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
        await EnsureBookIsAvailable(bookUUID);

        Borrow newBorrow = CreateBorrow(patronUUID, bookUUID);

        await SaveBorrow(newBorrow);

        return newBorrow;
    }

    private async Task EnsureBookIsAvailable(Guid bookUUID)
    {
        var borrowsByBook = await _borrowRepository.GetBorrowsByBook(bookUUID, 0, int.MaxValue);
        var activeBorrowForBook = borrowsByBook
            .FirstOrDefault(borrow => borrow.Status == BorrowStatus.Borrowed || borrow.Status == BorrowStatus.Overdue);

        if (activeBorrowForBook != null)
        {
            throw new InvalidOperationException("The book is already borrowed or overdue.");
        }
    }

    private Borrow CreateBorrow(Guid patronUUID, Guid bookUUID)
    {
        Guid borrowUUID = Guid.NewGuid();
        BorrowStatus borrowStatus = BorrowStatus.Borrowed;
        DateTime borrowDate = DateTime.Today;
        DateTime dueDate = borrowDate.AddDays(15);

        return new Borrow(borrowUUID, patronUUID, bookUUID, borrowStatus, dueDate, borrowDate);
    }

    private async Task SaveBorrow(Borrow borrow)
    {
        await _borrowRepository.Save(borrow);
    }
}