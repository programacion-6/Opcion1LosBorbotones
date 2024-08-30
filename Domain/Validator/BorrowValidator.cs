using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Validator.Exceptions;
using Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;

namespace Opcion1LosBorbotones.Domain.Validator;

public class BorrowValidator
{
    public void ValidateBorrow(Borrow borrow)
    {
        if (borrow == null)
        {
            throw new BorrowException(
                "Borrow object cannot be null",
                SeverityLevel.Critical,
                "This error occurs when the Borrow object is not initialized. " +
                "Ensure that the Borrow object is properly instantiated and filled with the necessary data before passing it for validation.");
        }

        ValidateDueDate(borrow.DueDate);
        ValidateBorrowDate(borrow.BorrowDate);
    }

    private void ValidateDueDate(DateTime dueDate)
    {
        if (dueDate < DateTime.Today)
        {
            throw new BorrowException(
                "Due date must be in the future",
                SeverityLevel.Medium,
                "The due date for the borrow should be in the future. " +
                "Please provide a valid due date for the borrow.");
        }

        if (dueDate < DateTime.Today.AddDays(7))
        {
            throw new BorrowException(
                "Due date must be at least 7 days from borrow date",
                SeverityLevel.Medium,
                "The due date for the borrow should be at least 7 days from the borrow date. " +
                "Please provide a valid due date for the borrow.");
        }
    }

    private void ValidateBorrowDate(DateTime borrowDate)
    {
        if (borrowDate > DateTime.Today)
        {
            throw new BorrowException(
                "Borrow date cannot be in the future",
                SeverityLevel.Medium,
                "The borrow date should not be in the future. " +
                "Please provide a valid borrow date for the borrow.");
        }

        if (borrowDate < DateTime.Today.AddDays(-30))
        {
            throw new BorrowException(
                "Borrow date cannot be more than 30 days in the past",
                SeverityLevel.Medium,
                "The borrow date should not be more than 30 days in the past. " +
                "Please provide a valid borrow date for the borrow.");
        }
    }
}
