using Npgsql;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Datasource;
using Opcion1LosBorbotones.Infrastructure.Services;
using Opcion1LosBorbotones.Infrastructure.Services.Mapper;

namespace Opcion1LosBorbotones.Infrastructure.Datasource;

public class PatronDatasourceImplementation : IPatronDatasource
{
    private readonly string _connectionString = 
        "Host=localhost;Port=5432;Database=borbotones;Username=user;Password=password";

    public async Task<Patron> CreateAsync(Patron entity)
    {
        const string query = """
            INSERT INTO Patron (id, name, membershipNumber, contactDetails)
            VALUES (@Id, @Name, @MembershipNumber, @ContactDetails)
            RETURNING *;
        """;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", entity.Id);
        command.Parameters.AddWithValue("Name", entity.Name);
        command.Parameters.AddWithValue("MembershipNumber", entity.MembershipNumber);
        command.Parameters.AddWithValue("ContactDetails", entity.ContactDetails);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapperImplementation.ToPatronEntity(reader);
        }

        throw new Exception("Failed to create the patron.");
    }

    public async Task<Patron?> ReadAsync(Guid id)
    {
        const string query = "SELECT * FROM Patron WHERE id = @Id;";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapperImplementation.ToPatronEntity(reader);
        }

        return null;
    }

    public async Task<Patron> UpdateAsync(Patron entity)
    {
        const string query = """
            UPDATE Patron
            SET name = @Name,
                membershipNumber = @MembershipNumber,
                contactDetails = @ContactDetails
            WHERE id = @Id
            RETURNING *;
        """;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", entity.Id);
        command.Parameters.AddWithValue("Name", entity.Name);
        command.Parameters.AddWithValue("MembershipNumber", entity.MembershipNumber);
        command.Parameters.AddWithValue("ContactDetails", entity.ContactDetails);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapperImplementation.ToPatronEntity(reader);
        }

        throw new Exception("Failed to update the patron.");
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string query = "DELETE FROM Patron WHERE id = @Id;";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Patron>> GetPatronsByNameAsync(string name)
    {
        const string query = "SELECT * FROM Patron WHERE name = @Name;";

        var patrons = new List<Patron>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Name", name);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            patrons.Add(MapperImplementation.ToPatronEntity(reader));
        }

        return patrons;
    }

    public async Task<Patron?> GetPatronByMembershipAsync(long membershipNumber)
    {
        const string query = "SELECT * FROM Patron WHERE membershipNumber = @MembershipNumber;";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("MembershipNumber", membershipNumber);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapperImplementation.ToPatronEntity(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Patron>> GetPatronsByContactDetailsAsync(long contactDetails)
    {
        const string query = "SELECT * FROM Patron WHERE contactDetails = @ContactDetails;";

        var patrons = new List<Patron>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("ContactDetails", contactDetails);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            patrons.Add(MapperImplementation.ToPatronEntity(reader));
        }

        return patrons;
    }
}
