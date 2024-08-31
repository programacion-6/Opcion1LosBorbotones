using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Presentation.Renderer;
using Opcion1LosBorbotones.Presentation.Renderer.PatronFormatter;

namespace Opcion1LosBorbotones.Presentation;

public class PatronFormatterFactory : IEntityFormatterFactory<Patron>
{
    public Task<EntityFormatter<Patron>?> CreateDetailedFormatter(Patron? entity)
    {
        if (entity is not null)
        {
            var formatter = Task.FromResult<EntityFormatter<Patron>?>(
                new DetailedPatronFormatter(entity));

            return formatter;
        }

        return Task.FromResult<EntityFormatter<Patron>?>(null);
    }
}