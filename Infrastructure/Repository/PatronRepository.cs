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

    public async Task<bool> Delete(long membershipnumber)
    {
        const string query = "DELETE FROM Patron WHERE membershipnumber = @MembershipNumber";

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(query, new { MembershipNumber = membershipnumber });

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

    public async Task<Patron?> GetPatronByContactDetailsAsync(long contactDetails)
    {
        const string query = "SELECT * FROM Patron WHERE contactDetails = @ContactDetails";

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Patron>(query, new { ContactDetails = contactDetails });
    }

    public async Task<IEnumerable<Patron>> GetPatronsByNameAsync(string name, int pageNumber, int pageSize)
    {
        const string sql = @"
        SELECT * FROM Patron
        WHERE name = @Name
        ORDER BY Name
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY";

        int offset = (pageNumber - 1) * pageSize;

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Patron>(sql, new { Name = name, Offset = offset, PageSize = pageSize });
        }
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

        entity.ContactDetails = long.Parse($"591{entity.ContactDetails:D8}");

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

        entity.ContactDetails = long.Parse($"591{entity.ContactDetails:D8}");

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
