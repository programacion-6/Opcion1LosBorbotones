using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Presentation.Renderer;
using Opcion1LosBorbotones.Presentation.Renderer.BookFormatter;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BookFormatterFactory : IEntityFormatterFactory<Book>
{
    public void CreateDetailedFormatter(Book? entity)
{
    if (entity is not null)
    {
        var formatter = new DetailedBookFormatter(entity);

        var formatterString = formatter.ToString();

        var panel = new Panel(new Markup($"[bold green]{formatterString}[/]"))
        {
            Border = BoxBorder.Rounded,
        };
        AnsiConsole.Write(panel);
    }
    else
    {
        ConsoleMessageRenderer.RenderInfoMessage("No result found");
    }
}

}
