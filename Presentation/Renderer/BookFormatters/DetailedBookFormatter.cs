using Opcion1LosBorbotones.Domain;

namespace Opcion1LosBorbotones.Presentation.Renderer.BookFormatter;

public class DetailedBookFormatter : EntityFormatter<Book>
{
    public DetailedBookFormatter(Book entity) : base(entity)
    {
    }

    public override string ToString()
    {
        return $"[bold plum3]----- |{_entity.Title}| -----[/]\n" +
               $"[bold plum3]Author:[/] {_entity.Author}\n" +
               $"[bold plum3]ISBN:[/] {_entity.Isbn}\n" +
               $"[bold plum3]Genre:[/] {_entity.Genre}\n" +
               $"[bold plum3]Year:[/] {_entity.PublicationYear}\n";
    }
}