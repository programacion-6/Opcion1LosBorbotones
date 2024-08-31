using System.Text;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Presentation;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public class PatronBorrowReport : IReport<long>
{
    private readonly IBorrowRepository _repository;
    private IEntityFormatterFactory<Borrow> _formatterFactoryBorrow;

    public PatronBorrowReport(IBorrowRepository repository, IEntityFormatterFactory<Borrow> formatterFactoryBorrow)
    {
        _repository = repository;
        _formatterFactoryBorrow = formatterFactoryBorrow;
    }

    public async Task<string> GenerateReport(long patronMembershipNumber, int offset, int limit)
    {
        StringBuilder report = new StringBuilder();
        IEnumerable<Borrow> borrows = 
                    await _repository.GetBorrowsByPatron(patronMembershipNumber, offset, limit);

        foreach (var borrow in borrows)
        {
            
            var formatter = await _formatterFactoryBorrow.CreateDetailedFormatter(borrow);
            report.AppendLine(formatter.ToString());

            //AnsiConsole.MarkupLine($"[bold italic green]New borrow registered:[/] {formatter}");
        }

        return report.ToString();
    }
}