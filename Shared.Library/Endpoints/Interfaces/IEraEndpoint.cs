namespace Shared.Library.Endpoints.Interfaces;
public interface IEraEndpoint
{
    Task DeleteEraAsync(EraModel era);
    Task<List<EraModel>> GetAllErasAsync();
    Task<EraModel> GetEraByIdAsync(int id);
    Task<EraModel> InsertEraAsync(EraModel era);
    Task UpdateEraAsync(EraModel era);
}