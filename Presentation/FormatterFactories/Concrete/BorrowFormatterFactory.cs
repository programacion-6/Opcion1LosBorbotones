using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Presentation.Renderer;
using Opcion1LosBorbotones.Presentation.Renderer.BorrowFormatter;
using Opcion1LosBorbotones.Presentation.Renders;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation;

public class BorrowFormatterFactory : IEntityFormatterFactory<Borrow>
{
    private IBookRepository _bookRepository;
    private IPatronRepository _patronRepository;

    public BorrowFormatterFactory(IBookRepository bookRepository, IPatronRepository patronRepository)
    {
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public void CreateDetailedFormatter(Borrow? entity)
{
    if (entity is not null)
    {
        var formatter = new DetailedBorrowFormatter(entity, _bookRepository, _patronRepository);
        
        // Ejecutar la tarea de manera síncrona
        var task = formatter.BorrowRelatedData();
        try
        {
            task.Wait(); // Espera a que la tarea se complete
        }
        catch (AggregateException ex)
        {
            // Manejo de excepciones
            Console.WriteLine($"Error: {ex.InnerException?.Message}");
        }

        // Convertir el formatter a una cadena
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