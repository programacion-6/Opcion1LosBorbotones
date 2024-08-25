using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Infrastructure.Services.Fines;

public static class FineCalculation
{
    public static IFineCalculation GetFineCalculation(string calculationType)
    {
        return calculationType switch
        {
            "MonthlyFineCalculation" => new MonthlyFineCalculation(),
            "WeeklyFineCalculation" => new WeeklyFineCalculation(),
            "DailyFineCalculation" => new DailyFineCalculation(),
            "YearlyFineCalculation" => new YearlyFineCalculation(),
            _ => throw new ArgumentException("Invalid calculation type")
        };
    }
}
