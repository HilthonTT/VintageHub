namespace Server.Library.DataAccess.Internal.Interfaces;

public interface ISqlDataAccess
{
    void CommitTransaction();
    Task<List<T>> LoadDataAsync<T>(string storedProcedure, DynamicParameters parameters);
    Task<List<T>> LoadDataInTransactionAsync<T>(string storedProcedure, DynamicParameters parameters);
    Task<T> LoadFirstOrDefaultAsync<T>(string storedProcedure, DynamicParameters parameters);
    Task<T> LoadFirstOrDefaultInTransactionAsync<T>(string storedProcedure, DynamicParameters parameters);
    void RollbackTransaction();
    Task<int> SaveDataAsync(string storedProcedure, DynamicParameters parameters);
    Task<int> SaveDataInTransactionAsync(string storedProcedure, DynamicParameters parameters);
    void StartTransaction();
}