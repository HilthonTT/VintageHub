namespace VintageHub.Client.Templates;

public partial class ArtifactDetailsTemplate
{
    [Parameter]
    [EditorRequired]
    public ArtifactDisplayModel Artifact { get; set; }

    private ShoppingCartModel shoppingCart;
    private string imageSource = "";
    private string truncatedName = "";
    private string truncatedDescription = "";
    private bool canAddToCart = false;

    protected override async Task OnInitializedAsync()
    {
        TruncateTexts();
        imageSource = ImageEndpoint.GetImage(Artifact.ImageId);
        shoppingCart = await ShoppingCartStorage.GetShoppingCartAsync();
        canAddToCart = CanAddToCart();
    }

    private void TruncateTexts()
    {
        string artifactName = Artifact.Name;
        truncatedName = artifactName?.Length > 20 ? string.Concat(artifactName.AsSpan(0, 17), "...") : artifactName;

        string description = Artifact.Description;
        truncatedDescription = description?.Length > 38 ? string.Concat(description.AsSpan(0, 35), "...") : description;
    }

    private void OpenDetails()
    {
        Navigation.NavigateTo($"/Artifact/{Artifact.Id}", true);
    }

    private bool CanAddToCart()
    {
        var item = shoppingCart.CartItems.FirstOrDefault(x => x.Artifact.Id == Artifact.Id);
        if (item is not null || Artifact.Quantity <= 0)
        {
            return false;
        }

        return true;
    }

    private async Task AddToCartAsync()
    {
        if (canAddToCart is false || shoppingCart is null)
        {
            return;
        }

        canAddToCart = false;
        var newCartItem = new CartItemModel
        {
            Artifact = Artifact,
            Quantity = 1,
        };
        shoppingCart.CartItems.Add(newCartItem);
        await ShoppingCartStorage.SaveShoppingCartAsync(shoppingCart);
    }
}