namespace Shared.Library.DataAccess.Interfaces;
public interface IEraData
{
    Task<int> DeleteEraAsync(int id);
    Task<List<EraModel>> GetAllErasAsync();
    Task<EraModel> GetEraByIdAsync(int id);
    Task<int> InsertEraAsync(EraModel era);
    Task<int> UpdateEraAsync(EraModel era);
}