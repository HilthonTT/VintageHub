namespace Shared.Library.Endpoints.Interfaces;
public interface IArtifactEndpoint
{
    Task DeleteArtifactAsync(ArtifactModel artifact);
    Task<List<ArtifactDisplayModel>> GetAllArtifactsAsync();
    Task<ArtifactDisplayModel> GetArtifactByIdAsync(int id);
    Task<List<ArtifactDisplayModel>> GetArtifactByVendorIdAsync(int vendorId);
    Task<ArtifactDisplayModel> InsertArtifactAsync(ArtifactModel artifact);
    Task UpdateArtifactAsync(ArtifactModel artifact);
}