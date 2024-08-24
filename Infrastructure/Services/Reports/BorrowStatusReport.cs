using System.Text;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public class BorrowStatusReport : IReport<BorrowStatus>
{
    BorrowRepositoryImplementation repository = BorrowRepositoryImplementation.GetInstance();
    
    public async Task<string> GenerateReport(BorrowStatus borrowStatus, int offset, int limit)
    {
        StringBuilder report = new StringBuilder();
        IEnumerable<Borrow> borrows = await repository.GetBorrowsByStatus(borrowStatus, offset, limit);
        
        foreach (var borrow in borrows)
        {
            report.AppendLine(borrow.ToString());
        }

        return report.ToString();
    }
}