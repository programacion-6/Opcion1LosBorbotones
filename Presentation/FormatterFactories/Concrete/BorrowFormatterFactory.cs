using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Presentation.Renderer;
using Opcion1LosBorbotones.Presentation.Renderer.BorrowFormatter;

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

    public async Task<EntityFormatter<Borrow>?> CreateDetailedFormatter(Borrow? entity)
    {
        if (entity is not null)
        {
            var formatter = new DetailedBorrowFormatter(entity, _bookRepository, _patronRepository);
            await formatter.BorrowRelatedData();

            return formatter;
        }

        return null;
    }

}