namespace VintageHub.Server.Library.DataAccess.Internal.Interfaces;

public interface ISqlDataAccess
{
    void CommitTransaction();
    Task<List<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters);
    Task<List<T>> LoadDataInTransactionAsync<T, U>(string storedProcedure, U parameters);
    Task<T> LoadFirstOrDefaultAsync<T, U>(string storedProcedure, U parameters);
    Task<T> LoadFirstOrDefaultInTransactionAsync<T, U>(string storedProcedure, U parameters);
    void RollbackTransaction();
    Task SaveDataAsync<T>(string storedProcedure, T parameters);
    Task SaveDataInTransactionAsync<T>(string storedProcedure, T parameters);
    void StartTransaction();
}