using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Presentation.Renderer;
using Opcion1LosBorbotones.Presentation.Renderer.BookFormatter;

namespace Opcion1LosBorbotones.Presentation;

public class BookFormatterFactory : IEntityFormatterFactory<Book>
{
    public Task<IEntityFormatter<Book>?> CreateDetailedFormatter(Book? entity)
    {
        if (entity is not null)
        {
            var formatter = Task.FromResult<IEntityFormatter<Book>?>(
                new DetailedBookFormatter(entity));

            return formatter;
        }

        return Task.FromResult<IEntityFormatter<Book>?>(null);
    }
}
