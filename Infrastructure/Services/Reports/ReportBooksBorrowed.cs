using System.Text;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public class ReportBooksBorrowed : IReport
{
    BorrowRepositoryImplementation repository = new BorrowRepositoryImplementation(new BorrowDatasourceImplementation());
    
    public async Task<string> GenerateReport(BorrowStatus borrowStatus)
    {
        StringBuilder report = new StringBuilder();
        IEnumerable<Borrow> borrows = await repository.GetBorrowsByStatus(borrowStatus);
        
        foreach (var item in borrows)
        {
            report.AppendLine(item.ToString()); // Asumiendo que la clase Borrow tiene el método ToString() implementado
        }

        return report.ToString(); // Convertimos el StringBuilder a string
    }
}