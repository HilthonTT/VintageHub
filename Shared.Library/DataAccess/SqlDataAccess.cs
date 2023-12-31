﻿namespace Shared.Library.DataAccess;
public class SqlDataAccess : ISqlDataAccess
{
    private const string DbName = "VintageData";
    private readonly IConfiguration _config;
    private static IDbConnection _connection;
    private static IDbTransaction _transaction;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    private string GetConnectionString()
    {
        return _config.GetConnectionString(DbName);
    }

    private static void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();

        _transaction = null;
        _connection = null;
    }

    private static void MapProperty<T, U>(T primaryEntity, U secondaryObject)
    {
        var properties = typeof(T).GetProperties();
        var selectedProperty = properties.FirstOrDefault(
            x => x.PropertyType == secondaryObject.GetType() ||
            x.Name == secondaryObject.GetType().Name);

        selectedProperty?.SetValue(primaryEntity, secondaryObject);
    }

    public async Task<List<T>> LoadDataAsync<T>(string storedProcedure, DynamicParameters parameters)
    {
        string connectionString = GetConnectionString();

        using var connection = new SqlConnection(connectionString);
        var rows = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        return rows.ToList();
    }

    public async Task<List<T>> LoadDetailedDataAsync<T>(
        string splitOnColumn,
        string storedProcedure,
        DynamicParameters parameters,
        params object[] secondaryObjects)
    {
        string connectionString = GetConnectionString();
        using var connection = new SqlConnection(connectionString);

        var types = new List<Type> { typeof(T) };
        types.AddRange(secondaryObjects.Select(obj => obj.GetType()));

        var entities = await connection.QueryAsync(
            storedProcedure,
            types.ToArray(),
            map: (objects) =>
            {
                var primaryEntity = (T)objects[0];

                for (int i = 0; i < objects.Length; i++)
                {
                    MapProperty(primaryEntity, objects[i]);
                }

                return primaryEntity;
            },
            splitOn: splitOnColumn,
            param: parameters,
            commandType: CommandType.StoredProcedure);

        return entities.ToList();
    }

    public async Task<T> LoadFirstOrDefaultDetailedDataAsync<T>(
        string splitOnColumn,
        string storedProcedure,
        DynamicParameters parameters,
        params object[] secondaryObjects)
    {
        var rows = await LoadDetailedDataAsync<T>(splitOnColumn, storedProcedure, parameters, secondaryObjects);
        return rows.FirstOrDefault();
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