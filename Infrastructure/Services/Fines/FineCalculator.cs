using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Fines;

public class FineCalculator
{
    public static double FinesAmount = 1.5;

    public static double CalculateFines(double overdueDays)
    {
        return FinesAmount * overdueDays;
    }
}