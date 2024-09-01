using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Presentation.Renderer.PatronFormatter;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class PatronFormatterFactory : IEntityFormatterFactory<Patron>
{
    public void CreateDetailedFormatter(Patron? entity)
    {
        if (entity is not null)
        {
            var formatter = new DetailedPatronFormatter(entity);
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