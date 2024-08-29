using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation;

public interface IBorrowConsoleRenderer
{
    Guid GetPatronId();
    Guid GetBookId();
    void DisplayBorrowDetails(Borrow borrow);
    bool ConfirmBorrow();
}