using Npgsql;
using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Services.Mapper;

namespace Opcion1LosBorbotones.Infrastructure.Datasource;

public class FineDatasourceImplementation : IFineDatasource
{
    private readonly string _connectionString = 
        "Host=localhost;Port=5432;Database=borbotones;Username=user;Password=password";

    public async Task<Fine> CreateAsync(Fine entity)
    {
        const string query = @"
            INSERT INTO Fine (id, borrow, amount, isPaid, calculationType)
            VALUES (@Id, @Borrow, @Amount, @IsPaid, @CalculationType)
            RETURNING *";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", entity.Id);
        command.Parameters.AddWithValue("Borrow", entity.Borrow.Id);
        command.Parameters.AddWithValue("Amount", entity.Amount);
        command.Parameters.AddWithValue("IsPaid", entity.IsPaid);
        command.Parameters.AddWithValue("CalculationType", entity.Calculation.GetType().Name);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return FineMapper.ToEntity(reader);
        }

        throw new Exception("Failed to create the fine.");
    }

    public async Task<Fine?> ReadAsync(Guid id)
    {
        const string query = "SELECT * FROM Fine WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return FineMapper.ToEntity(reader);;
        }

        return null;
    }

    public async Task<Fine> UpdateAsync(Fine entity)
    {
        const string query = @"
            UPDATE Fine
            SET amount = @Amount, isPaid = @IsPaid, calculationType = @CalculationType
            WHERE id = @Id
            RETURNING *";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", entity.Id);
        command.Parameters.AddWithValue("Amount", entity.Amount);
        command.Parameters.AddWithValue("IsPaid", entity.IsPaid);
        command.Parameters.AddWithValue("CalculationType", entity.Calculation.GetType().Name); 

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return FineMapper.ToEntity(reader);
        }

        throw new Exception("Failed to update the fine.");
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string query = "DELETE FROM Fine WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Fine>> GetAllAsync(int offset, int limit)
    {
        const string query = "SELECT * FROM Fine LIMIT @Limit OFFSET @Offset";

        var fines = new List<Fine>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Limit", limit);
        command.Parameters.AddWithValue("Offset", offset);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            fines.Add(FineMapper.ToEntity(reader));
        }

        return fines;
    }

    public async Task<IEnumerable<Fine>> GetFinesByBorrowIdAsync(Guid borrowId)
    {
        const string query = "SELECT * FROM Fine WHERE borrow = @BorrowId";

        var fines = new List<Fine>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("BorrowId", borrowId);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            fines.Add(FineMapper.ToEntity(reader));
        }

        return fines;
    }

    public async Task<IEnumerable<Fine>> GetFinesByStatusAsync(bool isPaid)
    {
        const string query = "SELECT * FROM Fine WHERE isPaid = @IsPaid";

        var fines = new List<Fine>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("IsPaid", isPaid);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            fines.Add(FineMapper.ToEntity(reader));
        }

        return fines;
    }
}
