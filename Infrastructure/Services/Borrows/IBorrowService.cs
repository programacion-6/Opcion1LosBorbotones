using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Infrastructure.Services.Borrows;

public interface IBorrowService
{
    Task<Borrow> RegisterNewBorrow(Guid patronUUID, Guid bookUUID);
    Task<Guid> GetBookIdByISBN(long isbn);
    Task<Guid> GetPatronIdByMembershipNumber(long membershipNumber);
}