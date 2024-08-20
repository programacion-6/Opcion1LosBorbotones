namespace Opcion1LosBorbotones.Domain.Entity;

public class Borrow : IEntity
{
    public Guid Id { get; }
    public Patron Patron { get; }
    public Book Book { get; }
    public BorrowStatus Status { get; }
    public DateTime DueDate { get; }
    public DateTime BorrowDate { get; }

    public Borrow(Guid id, Patron patron, Book book, BorrowStatus status, DateTime dueDate, DateTime borrowDate)
    {
        Id = id;
        Patron = patron;
        Book = book;
        Status = status;
        DueDate = dueDate;
        BorrowDate = borrowDate;
    }
    
    public override string ToString()
    {
        return $"Patron: {Patron}, Book: {Book}, BorrowStatus: {Status}, DueDate: {DueDate:yyyy-MM-dd}, BorrowDate: {BorrowDate:yyyy-MM-dd}";
    }
}