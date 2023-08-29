namespace Shared.Library.Endpoints.Web.Interfaces;

public interface IWishlistEndpoint
{
    Task DeleteWishlistasync(WishlistModel wishlist);
    Task<List<ArtifactDisplayModel>> GetAllArtifactsInWishlistAsync(int userId);
    Task<List<WishlistModel>> GetAllWishlistsAsync(int userId);
    Task<List<ArtifactDisplayModel>> InsertWishlistAsync(WishlistModel wishlist);
}