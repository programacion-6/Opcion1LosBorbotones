using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain.Services;

public interface IFineCalculation
{
    double CalculateFine(Borrow borrow);
}
