using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Infrastructure.Services.Fines;

public class DailyFineCalculation : IFineCalculation
{
    private const double DailyFineRate = 1.00;

    public double CalculateFine(Borrow borrow)
    {
        if (borrow.Status != BorrowStatus.Overdue)
            return 0.0;

        var overdueDays = (DateTime.Now - borrow.DueDate).Days;
        return overdueDays * DailyFineRate;
    }
}
