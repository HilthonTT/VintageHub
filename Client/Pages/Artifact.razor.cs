namespace VintageHub.Client.Pages;

public partial class Artifact
{
    [Parameter]
    public int Id { get; set; }

    private int selectedQuantity = 1;
    private string artifactImageSource = "";
    private string vendorImageSource = "";
    private string errorMessage = "";
    private bool isLoading = true;
    private bool canAddToCart = false;
    private bool canAddToWishlist = false;
    private List<ArtifactDisplayModel> randomArtifacts;
    private List<ReviewDisplayModel> reviews;
    private ArtifactDisplayModel artifact;
    private UserModel loggedInUser;
    private ShoppingCartModel shoppingCart;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            loggedInUser = await AuthProvider.GetUserFromAuth(UserEndpoint);
            shoppingCart = await ShoppingCartStorage.GetShoppingCartAsync();
            await LoadArtifactAsync();
            await LoadRandomArtifactsAsync();
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }

        isLoading = false;
    }

    private async Task LoadArtifactAsync()
    {
        artifact = await ArtifactEndpoint.GetArtifactByIdAsync(Id);

        if (artifact is not null)
        {
            artifact.Quantity = GetArtifactQuantity(artifact);
            canAddToCart = CanAddToCart(artifact);
            artifactImageSource = ImageEndpoint.GetImage(artifact.ImageId);
            vendorImageSource = ImageEndpoint.GetImage(artifact.Vendor.ImageId);
            canAddToWishlist = await CanAddToWishlistAsync();
            reviews = await ReviewEndpoint.GetAllReviewsByArtifactId(artifact.Id);
        }
    }

    private async Task LoadRandomArtifactsAsync()
    {
        var random = new Random();
        var artifacts = await ArtifactEndpoint.GetAllArtifactsAsync();

        randomArtifacts = artifacts.OrderBy(x => random.Next()).Take(25).ToList();
    }

    private void LoadReviewPage()
    {
        if (artifact is null)
        {
            return;
        }

        Navigation.NavigateTo($"/Artifact/{artifact?.Id}/Review");
    }

    private void ClosePage()
    {
        Navigation.NavigateTo("/");
    }

    private void LoadAllArtifactsPage()
    {
        Navigation.NavigateTo("/Listing");
    }

    private void LoadCartPage()
    {
        Navigation.NavigateTo("/Cart");
    }

    private void LoadWishlistPage()
    {
        Navigation.NavigateTo("/Wishlist");
    }

    private void LoadVendorPage()
    {
        if (artifact.Vendor is null)
        {
            return;
        }

        Navigation.NavigateTo($"/Vendor/{artifact.Vendor.Id}");
    }

    private bool CanAddToCart(ArtifactDisplayModel artifact)
    {
        var item = shoppingCart.CartItems.FirstOrDefault(x => x.Artifact.Id == artifact.Id);
        if (item is not null || artifact.Quantity <= 0)
        {
            return false;
        }

        return true;
    }

    private int GetArtifactQuantity(ArtifactDisplayModel artifact)
    {
        if (shoppingCart is null)
        {
            return artifact.Quantity;
        }

        int itemCount = shoppingCart.CartItems.Where(x => x.Artifact.Id == artifact.Id).Count();
        artifact.Quantity -= itemCount;

        if (artifact.Quantity < 0)
        {
            artifact.Quantity = 0;
        }

        return artifact.Quantity;
    }

    private async Task<bool> CanAddToWishlistAsync()
    {
        if (artifact is null || loggedInUser is null)
        {
            return false;
        }

        var wishlists = await WishlistEndpoint.GetAllWishlistsAsync(loggedInUser.Id);
        var w = wishlists.FirstOrDefault(w => w.ArtifactId == artifact.Id);

        if (w is not null)
        {
            return false;
        }

        return true;
    }

    private async Task AddToWishlistAsync()
    {
        if (canAddToWishlist is false)
        {
            return;
        }

        canAddToWishlist = true;
        var newWishlist = new WishlistModel
        {
            ArtifactId = artifact.Id,
            UserId = loggedInUser.Id,
        };

        await WishlistEndpoint.InsertWishlistAsync(newWishlist);

        LoadWishlistPage();
    }

    private async Task AddToCartAsync()
    {
        if (loggedInUser is null)
        {
            Navigation.NavigateToLogin("authentication/login");
        }

        if (canAddToCart is false)
        {
            return;
        }

        artifact.Quantity -= selectedQuantity;
        var newCartItem = new CartItemModel
        {
            Artifact = artifact,
            Quantity = selectedQuantity,
        };

        shoppingCart.CartItems.Add(newCartItem);
        await ShoppingCartStorage.SaveShoppingCartAsync(shoppingCart);

        LoadCartPage();
    }
}