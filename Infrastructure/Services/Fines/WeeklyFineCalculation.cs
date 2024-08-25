using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Infrastructure.Services.Fines;

public class WeeklyFineCalculation : IFineCalculation
{
    private const double WeeklyFineRate = 5.00;

    public double CalculateFine(Borrow borrow)
    {
        if (borrow.Status != BorrowStatus.Overdue)
            return 0.0;

        var overdueWeeks = (DateTime.Now - borrow.DueDate).Days / 7;
        return overdueWeeks * WeeklyFineRate;
    }
}
