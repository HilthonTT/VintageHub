namespace Server.Library.DataAccess.Interfaces;
public interface IArtifactData
{
    Task<int> DeleteArtifactAsync(int id);
    Task<List<ArtifactModel>> GetAllArtifactsAsync();
    Task<List<ArtifactModel>> GetAllArtifactsByVendorIdAsync(int vendorId);
    Task<ArtifactModel> GetArtifactByIdAsync(int id);
    Task<int> InsertArtifactAsync(ArtifactModel artifact);
    Task<int> UpdateArtifactAsync(ArtifactModel artifact);
}