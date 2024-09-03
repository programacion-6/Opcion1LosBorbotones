using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Fines;
using Opcion1LosBorbotones.Presentation.Renderer;

namespace Opcion1LosBorbotones.Presentation.Renders.EntityFormatters.BorrowFormatters;

public class DetailedDebtFormatter : EntityDetailedFormatter<Borrow>
{
    private readonly IBookRepository _bookRepository;
    private readonly IPatronRepository _patronRepository;
    private Book? _book;
    private Patron? _patron;

    public DetailedDebtFormatter(Borrow entity, IBookRepository bookRepository, IPatronRepository patronRepository)
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
        string formattedEntity = ""; 
        
        if (_book == null || _patron == null)
        {
            BorrowRelatedData().Wait();
        }
        
        var bookTitle = _book?.Title ?? "Unknown Book";
        int overdueDays =  _entity.DueDate.Day - DateTime.Today.Day;

        if (overdueDays > 0)
        {
            double totalDebt = FineCalculator.CalculateFines(overdueDays);

            formattedEntity = $"[bold plum3]Book:[/] {bookTitle}\n" +
                   $"[bold plum3]Overdue:[/] {overdueDays} days\n" +
                   $"[bold plum3]Total debt:[/] $ {totalDebt}\n";
        }

        return formattedEntity;
    }
}