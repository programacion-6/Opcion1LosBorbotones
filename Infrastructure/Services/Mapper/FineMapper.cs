using Npgsql;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Repository;
using Opcion1LosBorbotones.Infrastructure.Services.Fines;

namespace Opcion1LosBorbotones.Infrastructure.Services.Mapper;

public class FineMapper : IMapper<Fine, NpgsqlDataReader>
{

    public static Fine ToEntity(NpgsqlDataReader response)
    {
        var _borrowDatasource = BorrowRepositoryImplementation.GetInstance();
        var borrowId = (Guid)response["borrow"];
        Borrow? borrow = _borrowDatasource.ReadAsync(borrowId).Result;
        if (borrow == null)
        {
            throw new Exception("Borrow record not found.");
        }
        
        return new Fine(
            (Guid)response["id"],
            borrow,
            (double)response["amount"],
            (bool)response["isPaid"],
            FineCalculation.GetFineCalculation((string)response["calculationType"])
        );
    }
}
