namespace Server.Library.DataAccess.Internal;
public class SqlDataAccess : ISqlDataAccess
{
    private const string DbName = "VintageData";
    private const string CacheName = nameof(SqlDataAccess);

    private readonly IMemoryCache _cache;
    private readonly IConfiguration _config;

    private static bool _isClosed;
    private static IDbConnection _connection;
    private static IDbTransaction _transaction;

    public SqlDataAccess(
        IMemoryCache cache,
        IConfiguration config)
    {
        _cache = cache;
        _config = config;
    }

    private string GetConnectionString()
    {
        string output = _cache.Get<string>(CacheName);
        if (string.IsNullOrWhiteSpace(output))
        {
            output = _config.GetConnectionString(DbName);
            _cache.Set(CacheName, output);
        }

        return output;
    }

    private static void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();

        _transaction = null;
        _connection = null;

        _isClosed = true;
    }

    public async Task<List<T>> LoadDataAsync<T>(string storedProcedure, DynamicParameters parameters)
    {
        string connectionString = GetConnectionString();

        using var connection = new SqlConnection(connectionString);
        var rows = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        return rows.ToList();
    }

    public async Task<T> LoadFirstOrDefaultAsync<T>(string storedProcedure, DynamicParameters parameters)
    {
        string connectionString = GetConnectionString();

        using var connection = new SqlConnection(connectionString);
        var row = await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        return row;
    }

    public async Task<int> SaveDataAsync(string storedProcedure, DynamicParameters parameters)
    {
        string connectionString = GetConnectionString();

        using var connection = new SqlConnection(connectionString);
        using var multi = await connection.QueryMultipleAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        return await multi.ReadSingleAsync<int>();
    }

    public void StartTransaction()
    {
        string connectionString = GetConnectionString();

        _connection = new SqlConnection(connectionString);
        _connection.Open();

        _transaction = _connection.BeginTransaction();

        _isClosed = false;
    }

    public async Task<int> SaveDataInTransactionAsync(string storedProcedure, DynamicParameters parameters)
    {
        using var multi = await _connection.QueryMultipleAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction);

        return await multi.ReadSingleAsync<int>();
    }

    public async Task<List<T>> LoadDataInTransactionAsync<T>(string storedProcedure, DynamicParameters parameters)
    {
        var rows = await _connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction);

        return rows.ToList();
    }

    public async Task<T> LoadFirstOrDefaultInTransactionAsync<T>(string storedProcedure, DynamicParameters parameters)
    {
        var row = await _connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction);

        return row;
    }

    public void CommitTransaction()
    {
        _transaction?.Commit();
        _connection?.Close();

        Dispose();
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _connection?.Close();

        Dispose();
    }
}