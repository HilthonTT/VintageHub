namespace VintageHub.Client.Pages;

public partial class Wishlist
{
    private UserModel loggedInUser;
    private List<ArtifactDisplayModel> artifacts;
    private List<WishlistModel> wishlists;
    private bool isLoading = true;
    protected override async Task OnInitializedAsync()
    {
        loggedInUser = await AuthProvider.GetUserFromAuth(UserEndpoint);
        artifacts = await WishlistEndpoint.GetAllArtifactsInWishlistAsync(loggedInUser.Id);
        wishlists = await WishlistEndpoint.GetAllWishlistsAsync(loggedInUser.Id);
        isLoading = false;
    }

    private bool IsDisabled()
    {
        if (artifacts?.Count <= 0)
        {
            return true;
        }

        return false;
    }

    private async Task RemoveFromWishlistAsync(ArtifactDisplayModel artifact)
    {
        var removedWishlist = wishlists.FirstOrDefault(w => w.ArtifactId == artifact.Id);
        var removedArtifact = artifacts.FirstOrDefault(a => a.Id == artifact.Id);
        wishlists.Remove(removedWishlist);
        artifacts.Remove(removedArtifact);
        await WishlistEndpoint.DeleteWishlistasync(removedWishlist);
    }

    private async Task ClearWishlistAsync()
    {
        isLoading = true;
        foreach (var w in wishlists)
        {
            await WishlistEndpoint.DeleteWishlistasync(w);
        }

        wishlists.Clear();
        isLoading = false;
    }
}