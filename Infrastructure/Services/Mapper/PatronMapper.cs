using Npgsql;
using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Infrastructure.Services.Mapper;

public class PatronMapper : IMapper<Patron, NpgsqlDataReader>
{
    public static Patron ToEntity(NpgsqlDataReader response)
    {
        return new Patron(
            (Guid)response["id"],
            (string)response["name"],
            (long)response["membershipnumber"],
            (long)response["contactdetails"]
        );
    }
}