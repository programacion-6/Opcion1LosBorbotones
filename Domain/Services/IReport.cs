using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Infrastructure.Services.Reports;

public interface IReport<T>
{
    Task<string> GenerateReport(T entity, int offset, int limit);
}