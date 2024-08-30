using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Infrastructure.Services.Fines;

public class YearlyFineCalculation : IFineCalculation
{
    private const double YearlyFineRate = 50.00;

    public double CalculateFine(Borrow borrow)
    {
        if (borrow.Status != BorrowStatus.Overdue)
            return 0.0;

        var overdueYears = (DateTime.Now - borrow.DueDate).Days / 365;
        return overdueYears * YearlyFineRate;
    }
}
