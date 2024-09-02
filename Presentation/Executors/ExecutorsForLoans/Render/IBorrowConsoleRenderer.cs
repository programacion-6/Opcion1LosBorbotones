using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Presentation;

public interface IBorrowConsoleRenderer
{
    void DisplayBorrowDetails(Borrow borrow);
    bool ConfirmBorrow();
    bool ConfirmReturn();
}