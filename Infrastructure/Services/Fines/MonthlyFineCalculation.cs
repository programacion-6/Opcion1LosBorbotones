using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Infrastructure.Services.Fines;

public class MonthlyFineCalculation : IFineCalculation
{
    private const double MonthlyFineRate = 20.00;

    public double CalculateFine(Borrow borrow)
    {
        if (borrow.Status != BorrowStatus.Overdue)
            return 0.0;

        var overdueMonths = (DateTime.Now.Year - borrow.DueDate.Year) * 12 + DateTime.Now.Month - borrow.DueDate.Month;
        return overdueMonths * MonthlyFineRate;
    }
}
