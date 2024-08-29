using System.Text;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public class PatronBorrowReport : IReport<Guid>
{
    private readonly IBorrowRepository _repository;

    public PatronBorrowReport(IBorrowRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> GenerateReport(Guid patronId, int offset, int limit)
    {
        StringBuilder report = new StringBuilder();
        IEnumerable<Borrow> borrows = await _repository.GetBorrowsByPatron(patronId, offset, limit);

        foreach (var borrow in borrows)
        {
            report.AppendLine(borrow.ToString());
        }

        return report.ToString();
    }
}