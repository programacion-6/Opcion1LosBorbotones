using Npgsql;
using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Infrastructure.Services;
using Opcion1LosBorbotones.Infrastructure.Services.Mapper;

namespace Opcion1LosBorbotones.Infrastructure.Datasource;

public class BookDatasourceImplementation : IBookDatasource
{
    private readonly string _connectionString = 
        "Host=localhost;Port=5432;Database=borbotones;Username=user;Password=password";

    public async Task<Book> CreateAsync(Book entity)
    {
        const string query = """
                                 INSERT INTO Book (
                                     id, title, author, isbn, genre, publicationYear
                                 ) VALUES (
                                     @Id, @Title, @Author, @Isbn, @Genre, @PublicationYear
                                 ) RETURNING *
                                 """;
            
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", entity.Id);
        command.Parameters.AddWithValue("Title", entity.Title);
        command.Parameters.AddWithValue("Author", entity.Author);
        command.Parameters.AddWithValue("Isbn", entity.Isbn);
        command.Parameters.AddWithValue("Genre", (int)entity.Genre);
        command.Parameters.AddWithValue("PublicationYear", entity.PublicationYear);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapperImplementation.ToBookEntity(reader);
        }

        throw new Exception("Failed to create the book.");
    }

    public async Task<Book?> ReadAsync(Guid id)
    {
        const string query = @"
            SELECT * FROM Book
            WHERE id = @Id";
            
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapperImplementation.ToBookEntity(reader);
        }

        return null;
    }

    public async Task<Book> UpdateAsync(Book entity)
    {
        const string query = """
                                 UPDATE Book
                                 SET title = @Title,
                                     author = @Author,
                                     isbn = @Isbn,
                                     genre = @Genre,
                                     publicationYear = @PublicationYear
                                 WHERE id = @Id
                                 RETURNING *
                             """;
            
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", entity.Id);
        command.Parameters.AddWithValue("Title", entity.Title);
        command.Parameters.AddWithValue("Author", entity.Author);
        command.Parameters.AddWithValue("Isbn", entity.Isbn);
        command.Parameters.AddWithValue("Genre", (int)entity.Genre);
        command.Parameters.AddWithValue("PublicationYear", entity.PublicationYear);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapperImplementation.ToBookEntity(reader);
        }

        throw new Exception("Failed to update the book.");
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string query = @"
            DELETE FROM Book
            WHERE id = @Id";
            
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Book>> GetBooksByTitleAsync(string title)
    {
        const string query = @"
            SELECT * FROM Book
            WHERE title = @Title";
            
        var books = new List<Book>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Title", title);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            books.Add(MapperImplementation.ToBookEntity(reader));
        }

        return books;
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
    {
        const string query = @"
            SELECT * FROM Book
            WHERE author = @Author";
            
        var books = new List<Book>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Author", author);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            books.Add(MapperImplementation.ToBookEntity(reader));
        }

        return books;
    }

    public async Task<Book?> GetBookByIsbnAsync(long isbn)
    {
        const string query = @"
            SELECT * FROM Book
            WHERE isbn = @Isbn";
            
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Isbn", isbn);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapperImplementation.ToBookEntity(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Book>> GetBooksByGenreAsync(BookGenre genre)
    {
        const string query = @"
            SELECT * FROM Book
            WHERE genre = @Genre";
            
        var books = new List<Book>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Genre", (int)genre);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            books.Add(MapperImplementation.ToBookEntity(reader));
        }

        return books;
    }

    public async Task<IEnumerable<Book>> GetBooksByPublicationYearAsync(DateTime publicationYear)
    {
        const string query = @"
            SELECT * FROM Book
            WHERE publicationYear = @PublicationYear";
            
        var books = new List<Book>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("PublicationYear", publicationYear);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            books.Add(MapperImplementation.ToBookEntity(reader));
        }

        return books;
    }
}