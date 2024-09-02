using Dapper;
using Npgsql;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class BorrowRepository : IBorrowRepository
{
    private readonly string _connectionString;

    public BorrowRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> Delete(long id)
    {
        const string query = "DELETE FROM Borrow WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(query, new { Id = id });

        return result > 0;
    }

    public async Task<IEnumerable<Borrow>> GetAll()
    {
        const string query = "SELECT * FROM Borrow";

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Borrow>(query);
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByBook(Guid bookId, int offset, int limit)
    {
        const string query = @"
            SELECT 
                id AS Id, 
                patron AS PatronId, 
                book AS BookId, 
                borrowStatus AS Status, 
                dueDate AS DueDate, 
                borrowDate AS BorrowDate 
            FROM Borrow  
            WHERE book = @BookId 
            LIMIT @Limit OFFSET @Offset";

        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Borrow>(query, new
        {
            BookId = bookId,
            Limit = limit,
            Offset = offset
        });
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByBorrowDate(DateTime borrowDate, int offset, int limit)
    {
        const string query = @"
            SELECT 
                id AS Id, 
                patron AS PatronId, 
                book AS BookId, 
                borrowStatus AS Status, 
                dueDate AS DueDate, 
                borrowDate AS BorrowDate 
            FROM Borrow  
            WHERE borrowDate = @BorrowDate 
            LIMIT @Limit OFFSET @Offset";

        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Borrow>(query, new
        {
            BorrowDate = borrowDate,
            Limit = limit,
            Offset = offset
        });
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByDueDate(DateTime dueDate, int offset, int limit)
    {
        const string query = @"
            SELECT 
                id AS Id, 
                patron AS PatronId, 
                book AS BookId, 
                borrowStatus AS Status, 
                dueDate AS DueDate, 
                borrowDate AS BorrowDate 
            FROM Borrow 
            WHERE dueDate = @DueDate 
            LIMIT @Limit OFFSET @Offset";

        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Borrow>(query, new
        {
            DueDate = dueDate,
            Limit = limit,
            Offset = offset
        });
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByPatron(long patronMembershipNumber, int pageNumber, int pageSize)
    {
        const string query = @"
        SELECT 
            b.id AS Id, 
            b.patron AS PatronId, 
            b.book AS BookId, 
            b.borrowStatus AS Status, 
            b.dueDate AS DueDate, 
            b.borrowDate AS BorrowDate 
        FROM Borrow b
        JOIN Patron p ON b.patron = p.id
        WHERE p.MembershipNumber = @MembershipNumber
        ORDER BY b.borrowStatus
        LIMIT @Limit OFFSET @Offset";

        int offset = (pageNumber - 1) * pageSize;

        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Borrow>(query, new
        {
            MembershipNumber = patronMembershipNumber,
            Limit = pageSize,
            Offset = offset
        });
    }

    public async Task<IEnumerable<Borrow>> GetBorrowsByStatus(BorrowStatus status, int pageNumber, int pageSize)
    {
        const string sql = @"
            SELECT id AS Id, 
                patron AS PatronId, 
                book AS BookId, 
                borrowStatus AS Status, 
                dueDate AS DueDate, 
                borrowDate AS BorrowDate 
            FROM Borrow 
            WHERE borrowStatus = @Status 
            ORDER BY borrowStatus
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        int offset = (pageNumber - 1) * pageSize;

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Borrow>(sql, new { Status = (int)status, Offset = offset, PageSize = pageSize });
        }
    }

    public async Task<Borrow?> GetById(Guid id)
    {
        const string query = "SELECT * FROM Borrow WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Borrow>(query, new { Id = id });
    }

    public async Task<bool> Save(Borrow entity)
    {
        const string query = @"
        INSERT INTO Borrow (
            id, patron, book, borrowStatus, dueDate, borrowDate
        ) VALUES (
            @Id, @PatronId, @BookId, @BorrowStatus, @DueDate, @BorrowDate
        )";

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(query, new
        {
            entity.Id,
            entity.PatronId,
            entity.BookId,
            BorrowStatus = (int)entity.Status,
            entity.DueDate,
            entity.BorrowDate
        });

        return result > 0;
    }

    public async Task<bool> Update(Borrow entity)
    {
        const string query = @"
            UPDATE Borrow
            SET patron = @PatronId, book = @BookId, borrowStatus = @BorrowStatus, 
                dueDate = @DueDate, borrowDate = @BorrowDate
            WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(query, new
        {
            entity.Id,
            entity.PatronId,
            entity.BookId,
            BorrowStatus = (int)entity.Status,
            entity.DueDate,
            entity.BorrowDate
        });

        return result > 0;
    }

    public async Task<bool> UpdateBorrowStatus(Guid borrowId, BorrowStatus newStatus)
    {
        const string query = @"
        UPDATE Borrow
        SET borrowStatus = @BorrowStatus
        WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(query, new
        {
            Id = borrowId,
            BorrowStatus = (int)newStatus
        });

        return result > 0;
    }

}
