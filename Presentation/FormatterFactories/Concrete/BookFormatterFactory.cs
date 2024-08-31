using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Presentation.Renderer;
using Opcion1LosBorbotones.Presentation.Renderer.BookFormatter;

namespace Opcion1LosBorbotones.Presentation;

public class BookFormatterFactory : IEntityFormatterFactory<Book>
{
    public Task<EntityFormatter<Book>?> CreateDetailedFormatter(Book? entity)
    {
        if (entity is not null)
        {
            var formatter = Task.FromResult<EntityFormatter<Book>?>(
                new DetailedBookFormatter(entity));

            return formatter;
        }

        return Task.FromResult<EntityFormatter<Book>?>(null);
    }
}
