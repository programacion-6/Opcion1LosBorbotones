using Npgsql;
using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Infrastructure.Services.Mapper;

public class BorrowMapper : IMapper<Borrow, NpgsqlDataReader>
{
    public static Borrow ToEntity(NpgsqlDataReader response)
    {
        return new Borrow(
            (Guid)response["id"],
            (Guid)response["patron"],
            (Guid)response["book"],
            (BorrowStatus)(int)response["borrowstatus"]-1,
            (DateTime)response["duedate"],
            (DateTime)response["borrowdate"]
        );
    }
}