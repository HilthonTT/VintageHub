namespace Client.Library.Endpoints.Interfaces;

public interface IWishlistEndpoint
{
    Task DeleteWishlistasync(WishlistModel wishlist);
    Task<List<ArtifactModel>> GetAllArtifactsInWishlistAsync(int userId);
    Task<List<ArtifactModel>> InsertWishlistAsync(WishlistModel wishlist);
}