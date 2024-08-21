using System.Text;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public class ReportBooksStatus : IReport
{
    BorrowRepositoryImplementation repository = new BorrowRepositoryImplementation(new BorrowDatasourceImplementation());
    
    public async Task<string> GenerateBorrowStatusReport(BorrowStatus borrowStatus)
    {
        StringBuilder report = new StringBuilder();
        IEnumerable<Borrow> borrows = await repository.GetBorrowsByStatus(borrowStatus);
        
        foreach (var borrow in borrows)
        {
            report.AppendLine(borrow.ToString());
        }

        return report.ToString();
    }

    public Task<string> GeneratePatronBorrowReport(Guid patronId)
    {
        throw new NotSupportedException("GenerateBorrowStatusReport is not supported in GeneratePatronBorrowReport.");
    }
}