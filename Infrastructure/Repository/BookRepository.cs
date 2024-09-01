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

    public async Task<IEnumerable<Book>> GetBooksByAuthor(string author, int pageNumber, int pageSize)
    {
        const string sql = @"
            SELECT * FROM Book
            WHERE author = @Author
            ORDER BY Author
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        int offset = (pageNumber - 1) * pageSize;

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Book>(sql, new { Author = author, Offset = offset, PageSize = pageSize });
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByGenre(string genre, int pageNumber, int pageSize)
    {
        const string sql = @"
            SELECT * FROM Book
            WHERE genre = @Genre
            ORDER BY Genre
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        int offset = (pageNumber - 1) * pageSize;

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Book>(sql, new { Genre = genre, Offset = offset, PageSize = pageSize });
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByTitle(string title, int pageNumber, int pageSize)
    {
        const string sql = @"
        SELECT * FROM Book
        WHERE title = @Title
        ORDER BY Title
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY";

        int offset = (pageNumber - 1) * pageSize;

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Book>(sql, new { Title = title, Offset = offset, PageSize = pageSize });
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
