using Npgsql;
using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Infrastructure.Services.Mapper;

public class MapperImplementation : IMapper<NpgsqlDataReader>
{
    public static Book ToBookEntity(NpgsqlDataReader response)
    {
        return new Book(
            (Guid)response["id"],
            (string)response["title"],
            (string)response["author"],
            (long)response["isbn"],
            (BookGenre)(int)response["genre"],
            (DateTime)response["publicationyear"]
        );
    }

    public static Patron ToPatronEntity(NpgsqlDataReader response)
    {
        return new Patron(
            (Guid)response["id"],
            (string)response["name"],
            (long)response["membershipnumber"],
            (long)response["contactdetails"]
        );
    }
}