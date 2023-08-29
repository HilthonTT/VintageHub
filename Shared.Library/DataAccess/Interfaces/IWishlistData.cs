namespace Shared.Library.DataAccess.Interfaces;

public interface IWishlistData
{
    Task<int> DeleteWishlistAsync(int id);
    Task<List<ArtifactDisplayModel>> GetAllArtifactsInWishlistAsync(int userId);
    Task<List<WishlistModel>> GetAllWishlistsByUserIdAsync(int userId);
    Task<int> InsertWishlistAsync(WishlistModel wishlist);
}