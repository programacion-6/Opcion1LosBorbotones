using System.Text;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public class PatronBorrowReport : IReport<Guid>
{
    BorrowRepositoryImplementation repository = new BorrowRepositoryImplementation(new BorrowDatasourceImplementation());

    public async Task<string> GenerateReport(Guid patronId)
    {
        StringBuilder report = new StringBuilder();
        IEnumerable<Borrow> borrows = await repository.GetBorrowsByPatron(patronId);

        foreach (var borrow in borrows)
        {
            report.AppendLine(borrow.ToString());
        }

        return report.ToString();
    }
}