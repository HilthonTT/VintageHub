namespace Server.Library.DataAccess.Interfaces;

public interface IWishlistData
{
    Task<int> DeleteWishlistAsync(int id);
    Task<List<ArtifactModel>> GetAllArtifactsInWishlistAsync(int userId);
    Task<int> InsertWishlistAsync(WishlistModel wishlist);
}