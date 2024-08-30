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
        ValidateNullOrWhiteSpace(title, "Title");
        ValidateLengthString(title, 100, "Title");
    }

    private void ValidateAuthor(string author)
    {
        ValidateNullOrWhiteSpace(author, "Author");

        if (!author.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
        {
            throw new BookException(
                "Author name must contain only letters.",
                SeverityLevel.Medium,
                "The author's name should consist of alphabetic characters only. " +
                "Please verify the author's name and ensure it does not contain any special characters or numbers.");
        }
    }

    private void ValidateISBN(long isbn)
    {
        var isbnString = isbn.ToString();

        if (isbnString.Length != 10 && isbnString.Length != 13)
        {
            throw new BookException(
                "ISBN must be either 10 or 13 digits long.",
                SeverityLevel.Medium,
                "Ensure that the ISBN is either 10 or 13 digits without any other characters."
            );
        }
    }

    private void ValidateGenreBook(string genre)
    {
        ValidateNullOrWhiteSpace(genre, "Genre");

        if (!genre.All(char.IsLetter))
        {
            throw new BookException(
                "Genre must contain only letters.",
                SeverityLevel.Medium,
                "The genre should consist of alphabetic characters only. " +
                "Please verify the genre and ensure it does not contain any special characters or numbers.");
        }
    }

    private void ValidatePublicationYear(DateTime publicationYear)
    {
        if (publicationYear > DateTime.Now)
        {
            throw new BookException(
                "Publication year cannot be in the future.",
                SeverityLevel.Medium,
                "The publication year should not be in the future. " +
                "Please verify the publication year and ensure it is a valid date in the past or present.");
        }

        if (publicationYear.Year < 1455)
        {
            throw new BookException(
                "Publication year is too early.",
                SeverityLevel.Medium,
                "The publication year is too early. " +
                "Please verify the publication year and ensure it is a valid date after the invention of the printing press.");
        }
    }

    private void ValidateLengthString(string value, int maxLength, string fieldName)
    {
        if (value.Length > maxLength)
        {
            throw new BookException(
                $"{fieldName} exceeds maximum length of {maxLength} characters.",
                SeverityLevel.Medium,
                $"The {fieldName} should not exceed {maxLength} characters in length. " +
                $"Please shorten the {fieldName} to ensure it meets the length requirements.");
        }
    }

    private void ValidateNullOrWhiteSpace(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BookException(
                $"{fieldName} cannot be null, empty, or whitespace.",
                SeverityLevel.Medium,
                $"The {fieldName} field is mandatory and must contain meaningful text. " +
                $"Ensure that the {fieldName} field is populated with a valid, non-empty string.");
        }
    }
}
