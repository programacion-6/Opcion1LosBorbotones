using Dapper;
using Npgsql;
using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class BookRepository : IBookRepository
{
    private readonly string _connectionString;

    public BookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> Delete(long isbn)
    {
        const string sql = "DELETE FROM Book WHERE isbn = @Isbn";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, new { Isbn = isbn });
            return affected > 0;
        }
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        const string sql = "SELECT * FROM Book";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Book>(sql);
        }
    }

    public async Task<Book?> GetBookByISBN(long isbn)
    {
        const string sql = "SELECT * FROM Book WHERE isbn = @Isbn";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<Book>(sql, new { isbn });
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthor(string author, int offset, int limit)
    {
        const string sql = @"
            SELECT * FROM Book
            WHERE author = @Author
            LIMIT @Limit OFFSET @Offset";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Book>(sql, new { Author = author, Limit = limit, Offset = offset });
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByGenre(string genre, int offset, int limit)
    {
         const string sql = @"
            SELECT * FROM Book
            WHERE genre = @Genre
            LIMIT @Limit OFFSET @Offset";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Book>(sql, new { Genre = genre, Limit = limit, Offset = offset });
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByTitle(string title, int offset, int limit)
    {
        const string sql = @"
            SELECT * FROM Book
            WHERE title = @Title
            LIMIT @Limit OFFSET @Offset";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Book>(sql, new { Title = title, Limit = limit, Offset = offset });
        }
    }

    public async Task<Book?> GetById(Guid id)
    {
        const string sql = "SELECT * FROM Book WHERE id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<Book>(sql, new { Id = id });
        }
    }

    public async Task<bool> Save(Book entity)
    {
       const string sql = @"
                INSERT INTO Book (id, title, author, isbn, genre, publicationYear)
                VALUES (@Id, @Title, @Author, @Isbn, @Genre, @PublicationYear)";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, entity);
            return affected > 0;
        }
    }

    public async Task<bool> Update(Book entity)
    {
        const string sql = @"
                UPDATE Book
                SET title = @Title, author = @Author, isbn = @ISBN, 
                    genre = @Genre, publicationYear = @PublicationYear
                WHERE id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, entity);
            return affected > 0;
        }
    }

}
