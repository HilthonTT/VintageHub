using Client.Library.Models;

namespace Client.Library.Endpoints.Interfaces;
public interface IArtifactEndpoint
{
    Task DeleteArtifactAsync(ArtifactModel artifact);
    Task<List<ArtifactModel>> GetAllArtifactsAsync();
    Task<ArtifactModel> GetArtifactByIdAsync(int id);
    Task<ArtifactModel> GetArtifactByVendorIdAsync(int vendorId);
    Task InsertArtifactAsync(ArtifactModel artifact);
    Task UpdateArtifactAsync(ArtifactModel artifact);
}