namespace Shared.Library.DataAccess.Interfaces;
public interface IArtifactData
{
    Task<int> DeleteArtifactAsync(int id);
    Task<List<ArtifactDisplayModel>> GetAllArtifactsAsync();
    Task<List<ArtifactDisplayModel>> GetAllArtifactsByVendorIdAsync(int vendorId);
    Task<ArtifactDisplayModel> GetArtifactByIdAsync(int id);
    Task<int> InsertArtifactAsync(ArtifactModel artifact);
    Task<int> UpdateArtifactAsync(ArtifactModel artifact);
}