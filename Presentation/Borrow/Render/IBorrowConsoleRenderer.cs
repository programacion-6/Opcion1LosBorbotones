using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation;

public interface IBorrowConsoleRenderer
{
    long GetMembershipNumber();
    long GetISBN();
    void DisplayBorrowDetails(Borrow borrow);
    bool ConfirmBorrow();
}