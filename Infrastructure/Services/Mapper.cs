using Npgsql;
using Opcion1LosBorbotones.Domain;
namespace Opcion1LosBorbotones.Infrastructure.Services;

public class Mapper
{
    public static Book ReaderToBookEntity(NpgsqlDataReader reader)
    {
        return new Book(
            (Guid)reader["id"],
            (string)reader["title"],
            (string)reader["author"],
            (long)reader["isbn"],
            (BookGenre)(int)reader["genre"],
            (DateTime)reader["publicationyear"]
        );
    }
}