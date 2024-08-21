using Npgsql;
using Opcion1LosBorbotones.Domain;

namespace Opcion1LosBorbotones.Infrastructure.Services.Mapper;

public class BookMapper : IMapper<Book, NpgsqlDataReader>
{
    public static Book ToEntity(NpgsqlDataReader response)
    {
        return new Book(
            (Guid)response["id"],
            (string)response["title"],
            (string)response["author"],
            (long)response["isbn"],
            (BookGenre)(int)response["genre"]-1,
            (DateTime)response["publicationyear"]
        );
    }
}