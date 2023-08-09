using VintageHub.Server.Library.Models;

namespace VintageHub.Server.Library.DataAccess.Interfaces;
public interface IArtifactData
{
    Task<int> DeleteArtifactAsync(ArtifactModel artifact);
    Task<List<ArtifactModel>> GetAllArtifactsAsync();
    Task<ArtifactModel> GetArtifactAsync(int id);
    Task<int> InsertArtifactAsync(ArtifactModel artifact);
    Task<int> UpdateArtifactAsync(ArtifactModel artifact);
}