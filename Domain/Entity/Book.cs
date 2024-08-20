using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain;

public class Book : IEntity
{
    public Guid Id { get; }
    public string Title { get; }
    public string Author { get; }
    public string Isbn { get; }
    public BookGenre Genre { get; }
    public DateTime PublicationYear { get; }
    
    
    public Book(Guid id, string title, string author, string isbn, BookGenre genre, DateTime publicationYear)
    {
        Id = id;
        Title = title;
        Author = author;
        Isbn = isbn;
        Genre = genre;
        PublicationYear = publicationYear;
    }
    
}