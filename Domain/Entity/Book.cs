using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain;

public class Book : IEntity
{
    public Guid Id { get; set; }
    public string Title { get; }
    public string Author { get; }
    public long Isbn { get; }
    public string Genre { get; }
    public DateTime PublicationYear { get; }
    
    public Book()
    {
    }
    
    public Book(Guid id, string title, string author, long isbn, string genre, DateTime publicationYear)
    {
        Id = id;
        Title = title;
        Author = author;
        Isbn = isbn;
        Genre = genre;
        PublicationYear = publicationYear;
    }

}