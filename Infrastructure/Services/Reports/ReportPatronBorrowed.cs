using System.Text;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public class ReportPatronBorrowed : IReport
{
    BorrowRepositoryImplementation repository = new BorrowRepositoryImplementation(new BorrowDatasourceImplementation());

    public async Task<string> GeneratePatronBorrowReport(Guid patronId)
    {
        StringBuilder report = new StringBuilder();
        IEnumerable<Borrow> borrows = await repository.GetBorrowsByPatron(patronId);

        foreach (var borrow in borrows)
        {
            report.AppendLine(borrow.ToString());
        }

        return report.ToString();
    }
    
    public Task<string> GenerateBorrowStatusReport(BorrowStatus borrowStatus)
    {
        throw new NotSupportedException("ReportPatronBorrowed is not supported in GenerateBorrowStatusReport.");
    }
}