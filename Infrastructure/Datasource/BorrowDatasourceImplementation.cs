using Npgsql;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Infrastructure.Services.Mapper;

namespace Opcion1LosBorbotones.Infrastructure.Datasource;

public class BorrowDatasourceImplementation : IBorrowDatasource
{
    private readonly string _connectionString = 
        "Host=localhost;Port=5432;Database=borbotones;Username=user;Password=password";

    public async Task<Borrow> CreateAsync(Borrow entity)
    {
        const string query = """
            INSERT INTO Borrow (
                id, patron, book, borrowStatus, dueDate, borrowDate
            ) VALUES (
                @Id, @Patron, @Book, @BorrowStatus, @DueDate, @BorrowDate
            ) RETURNING *
        """;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", entity.Id);
        command.Parameters.AddWithValue("Patron", entity.PatronId);
        command.Parameters.AddWithValue("Book", entity.BookId);
        command.Parameters.AddWithValue("BorrowStatus", (int)entity.Status + 1);
        command.Parameters.AddWithValue("DueDate", entity.DueDate);
        command.Parameters.AddWithValue("BorrowDate", entity.BorrowDate);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return BorrowMapper.ToEntity(reader);
        }

        throw new Exception("Failed to create the borrow record.");
    }

    public async Task<Borrow?> ReadAsync(Guid id)
    {
        const string query = "SELECT * FROM Borrow WHERE id = @Id";
        
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return BorrowMapper.ToEntity(reader);
        }

        return null;
    }

    public async Task<Borrow> UpdateAsync(Borrow entity)
    {
        const string query = """
            UPDATE Borrow
            SET patron = @Patron, book = @Book, borrowStatus = @BorrowStatus, 
                dueDate = @DueDate, borrowDate = @BorrowDate
            WHERE id = @Id
            RETURNING *
        """;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", entity.Id);
        command.Parameters.AddWithValue("Patron", entity.PatronId);
        command.Parameters.AddWithValue("Book", entity.BookId);
        command.Parameters.AddWithValue("BorrowStatus", (int)entity.Status + 1);
        command.Parameters.AddWithValue("DueDate", entity.DueDate);
        command.Parameters.AddWithValue("BorrowDate", entity.BorrowDate);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return BorrowMapper.ToEntity(reader);
        }

        throw new Exception("Failed to update the borrow record.");
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string query = "DELETE FROM Borrow WHERE id = @Id";
        
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByPatron(Guid patronId, int offset, int limit)
    {
        const string query = @"
            SELECT * FROM Borrow 
            WHERE patron = @PatronId 
            LIMIT @Limit OFFSET @Offset";
        
        var borrows = new List<Borrow>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("PatronId", patronId);
        command.Parameters.AddWithValue("Limit", limit);
        command.Parameters.AddWithValue("Offset", offset);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            borrows.Add(BorrowMapper.ToEntity(reader));
        }

        return borrows;
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByBook(Guid bookId, int offset, int limit)
    {
        const string query = @"
            SELECT * FROM Borrow 
            WHERE book = @BookId 
            LIMIT @Limit OFFSET @Offset";
        
        var borrows = new List<Borrow>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("BookId", bookId);
        command.Parameters.AddWithValue("Limit", limit);
        command.Parameters.AddWithValue("Offset", offset);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            borrows.Add(BorrowMapper.ToEntity(reader));
        }

        return borrows;
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByStatus(BorrowStatus status, int offset, int limit)
    {
        const string query = @"
            SELECT * FROM Borrow 
            WHERE borrowStatus = @Status 
            LIMIT @Limit OFFSET @Offset";

        var borrows = new List<Borrow>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Status", (int)status+1);
        command.Parameters.AddWithValue("Limit", limit);
        command.Parameters.AddWithValue("Offset", offset);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            borrows.Add(BorrowMapper.ToEntity(reader));
        }

        return borrows;
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByDueDate(DateTime dueDate, int offset, int limit)
    {
        const string query = @"
            SELECT * FROM Borrow 
            WHERE dueDate = @DueDate 
            LIMIT @Limit OFFSET @Offset";

        var borrows = new List<Borrow>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("DueDate", dueDate);
        command.Parameters.AddWithValue("Limit", limit);
        command.Parameters.AddWithValue("Offset", offset);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            borrows.Add(BorrowMapper.ToEntity(reader));
        }

        return borrows;
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByBorrowDate(DateTime borrowDate, int offset, int limit)
    {
        const string query = @"
            SELECT * FROM Borrow 
            WHERE borrowDate = @BorrowDate 
            LIMIT @Limit OFFSET @Offset";

        var borrows = new List<Borrow>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("BorrowDate", borrowDate);
        command.Parameters.AddWithValue("Limit", limit);
        command.Parameters.AddWithValue("Offset", offset);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            borrows.Add(BorrowMapper.ToEntity(reader));
        }

        return borrows;
    }

    public async Task<IEnumerable<Borrow>> GetAllAsync(int offset, int limit)
    {
        const string query = @"
            SELECT * FROM Borrow 
            LIMIT @Limit OFFSET @Offset";

        var borrows = new List<Borrow>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Limit", limit);
        command.Parameters.AddWithValue("Offset", offset);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            borrows.Add(BorrowMapper.ToEntity(reader));
        }

        return borrows;
    }
}
