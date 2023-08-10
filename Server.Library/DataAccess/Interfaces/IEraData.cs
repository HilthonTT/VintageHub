namespace Server.Library.DataAccess.Interfaces;
public interface IEraData
{
    Task<int> DeleteEraAsync(EraModel era);
    Task<List<EraModel>> GetAllErasAsync();
    Task<EraModel> GetEraByIdAsync(int id);
    Task<int> InsertEraAsync(EraModel era);
    Task<int> UpdateEraAsync(EraModel era);
}