using System.Text;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public class BorrowStatusReport : IReport<BorrowStatus>
{
    private readonly IBorrowRepository _repository;

    public BorrowStatusReport(IBorrowRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<string> GenerateReport(BorrowStatus borrowStatus, int offset, int limit)
    {
        StringBuilder report = new StringBuilder();
        IEnumerable<Borrow> borrows = await _repository.GetBorrowsByStatus(borrowStatus, offset, limit);
        
        foreach (var borrow in borrows)
        {
            report.AppendLine(borrow.ToString());
        }

        return report.ToString();
    }
}