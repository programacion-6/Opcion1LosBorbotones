using Opcion1LosBorbotones.Domain.Services;

namespace Opcion1LosBorbotones.Domain.Entity;

public class Fine : IEntity
{
    public Guid Id { get; }
    public Borrow Borrow { get; }
    public double Amount { get; }
    public bool IsPaid { get; }
    public IFineCalculation Calculation { get; }

    public Fine(Guid id, Borrow borrow, double amount, bool IsPaid, IFineCalculation calculation)
    {
        Id = id;
        Borrow = borrow;
        Amount = amount;
        this.IsPaid = IsPaid;
        Calculation = calculation; 
    }
    
    public override string ToString()
    {
        return $"Fine: {Id}, Borrow: {Borrow}, Amount: {Amount}, IsPaid: {IsPaid}";
    }
}
