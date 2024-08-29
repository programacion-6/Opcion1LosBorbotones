using Dapper;
using Npgsql;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class PatronRepository : IPatronRepository
{
    private readonly string _connectionString;

    public PatronRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> Delete(Guid id)
    {
        const string query = "DELETE FROM Patron WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(query, new { Id = id });

        return result > 0;
    }

    public async Task<IEnumerable<Patron>> GetAll()
    {
        const string query = "SELECT * FROM Patron";

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Patron>(query);
    }

    public async Task<Patron?> GetById(Guid id)
    {
        const string query = "SELECT * FROM Patron WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Patron>(query, new { Id = id });
    }

    public async Task<Patron?> GetPatronByMembershipAsync(long membershipNumber)
    {
        const string query = "SELECT * FROM Patron WHERE membershipNumber = @MembershipNumber";

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Patron>(query, new { MembershipNumber = membershipNumber });
    }

    public async Task<IEnumerable<Patron?>> GetPatronsByContactDetailsAsync(long contactDetails, int offset, int limit)
    {
        const string query = @"
            SELECT * FROM Patron
            WHERE contactDetails = @ContactDetails
            LIMIT @Limit OFFSET @Offset;";

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Patron>(query, new
        {
            ContactDetails = contactDetails,
            Limit = limit,
            Offset = offset
        });
    }

    public async Task<IEnumerable<Patron>> GetPatronsByNameAsync(string name, int offset, int limit)
    {
        const string query = @"
            SELECT * FROM Patron
            WHERE name = @Name
            LIMIT @Limit OFFSET @Offset;";

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Patron>(query, new
        {
            Name = name,
            Limit = limit,
            Offset = offset
        });
    }

    public async Task<bool> Save(Patron entity)
    {
        const string query = @"
            INSERT INTO Patron (id, name, membershipNumber, contactDetails)
            VALUES (@Id, @Name, @MembershipNumber, @ContactDetails)
            ON CONFLICT (id) DO UPDATE SET
                name = @Name,
                membershipNumber = @MembershipNumber,
                contactDetails = @ContactDetails;";

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(query, new
        {
            entity.Id,
            entity.Name,
            entity.MembershipNumber,
            entity.ContactDetails
        });

        return result > 0;
    }

    public async Task<bool> Update(Patron entity)
    {
        const string query = @"
            UPDATE Patron
            SET name = @Name,
                membershipNumber = @MembershipNumber,
                contactDetails = @ContactDetails
            WHERE id = @Id;";

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(query, new
        {
            entity.Id,
            entity.Name,
            entity.MembershipNumber,
            entity.ContactDetails
        });

        return result > 0;
    }
}
