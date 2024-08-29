using Dapper;
using Npgsql;
using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Repository;

public class BookRepository : IBookRepository_
{
    private readonly string _connectionString = 
        "Host=localhost;Port=5432;Database=borbotones;Username=user;Password=password";

    public async Task<bool> Delete(Guid id)
    {
        const string sql = "DELETE FROM Book WHERE id = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            int affected = await connection.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }
    }

    public Task<IEnumerable<Book>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Book?> GetBookByISBN(long isbn)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book?>> GetBooksByAuthor(string author)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book?>> GetBooksByGenre(BookGenre genre)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book?>> GetBooksByTitle(string title)
    {
        throw new NotImplementedException();
    }

    public Task<Book?> GetById(Guid id)
    {
        throw new NotImplementedException();
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
