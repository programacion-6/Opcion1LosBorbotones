using Npgsql;

namespace Opcion1LosBorbotones.Domain.Data;

public class DatabaseConfig
{
    private readonly string _connectionString;

    public DatabaseConfig()
    {
        _connectionString = CreateConnectionString();
    }

    public string ConnectionString
    {
        get => _connectionString;
    }

    private string CreateConnectionString()
    {
        DotNetEnv.Env.Load();

        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        var connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={dbName}";
        
        return connectionString;
    }

    public NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
