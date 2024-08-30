using Opcion1LosBorbotones.Domain.Validator.Exceptions;
using Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;

namespace Opcion1LosBorbotones.Domain.Validator;

public class BookValidator
{
    public void ValidateBook(Book book)
    {
        if (book == null)
        {
            throw new BookException(
                "Book object cannot be null.",
                SeverityLevel.Critical,
                "This error indicates that the Book object was not initialized before being passed for validation. " +
                "Ensure that the Book object is properly instantiated and contains all required fields before attempting to validate it."
            );
        }

        ValidateTitle(book.Title);
        ValidateAuthor(book.Author);
        ValidateISBN(book.Isbn);
        ValidateGenreBook(book.Genre);
        ValidatePublicationYear(book.PublicationYear);
    }

    private void ValidateTitle(string title)
    {

    }

    private void ValidateAuthor(string author)
    {

    }

    private void ValidateISBN(long isbn)
    {

    }

    private void ValidateGenreBook(string genre)
    {

    }

    private void ValidatePublicationYear(DateTime publicationYear)
    {

    }
}
