namespace Opcion1LosBorbotones.Domain.Entity;

public class Borrow : IEntity
{
    public Guid Id { get; set; }
    public Guid PatronId { get; }
    public Guid BookId { get; }
    public BorrowStatus Status { get; }
    public DateTime DueDate { get; }
    public DateTime BorrowDate { get; }

    public Borrow(Guid id, Guid patronId, Guid bookId, BorrowStatus status, DateTime dueDate, DateTime borrowDate)
    {
        Id = id;
        PatronId = patronId;
        BookId = bookId;
        Status = status;
        DueDate = dueDate;
        BorrowDate = borrowDate;
    }
    
    public override string ToString()
    {
        return $"Patron: {PatronId}, Book: {BookId}, BorrowStatus: {Status}, DueDate: {DueDate:yyyy-MM-dd}, BorrowDate: {BorrowDate:yyyy-MM-dd}";
    }
}