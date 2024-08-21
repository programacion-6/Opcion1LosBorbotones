using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public interface IReport
{
    Task<string> GenerateReport(BorrowStatus borrowStatus);
}