using Opcion1LosBorbotones.Domain;

namespace Opcion1LosBorbotones.Presentation.Renderer.BookFormatter;

public class DetailedBookFormatter : IEntityFormatter<Book>
{
    public DetailedBookFormatter(Book entity) : base(entity)
    {
    }

    public override string ToString()
    {
        return $"Title: {_entity.Title}, Author: {_entity.Author}, ISBN: {_entity.Isbn}, Genre: {_entity.Genre}, Publication Year: {_entity.PublicationYear:yyyy-MM-dd}";
    }
}