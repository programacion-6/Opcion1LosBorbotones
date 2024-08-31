using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Presentation.Renderer.BorrowFormatter;

public class DetailedBorrowFormatter : EntityDetailedFormatter<Borrow>
{
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;
    private Book? _book;
    private Patron? _patron;

    public DetailedBorrowFormatter(Borrow entity, IBookRepository bookRepository, IPatronRepository patronRepository)
        : base(entity)
    {
        _bookRepository = bookRepository;
        _patronRepository = patronRepository;
    }

    public override async Task BorrowRelatedData()
    {
        if (Entity is not null)
        {
            _book = await _bookRepository.GetById(_entity.BookId);
            _patron = await _patronRepository.GetById(_entity.PatronId);
        }
    }

    public override string ToString()
    {
        var patronName = _patron?.Name ?? "[bold yellow][Unknown Patron][/]";
        var bookTitle = _book?.Title ?? "[bold yellow][Unknown Book][/]";

        return $"[bold plum3]Patron:[/] {patronName}\n" +
               $"[bold plum3]Book:[/] {bookTitle}\n" +
               $"[bold plum3]BorrowStatus:[/] {_entity.Status}\n";
    }
}