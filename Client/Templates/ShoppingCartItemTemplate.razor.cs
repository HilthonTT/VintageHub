namespace VintageHub.Client.Templates;

public partial class ShoppingCartItemTemplate
{
    [Parameter]
    [EditorRequired]
    public CartItemModel CartItem { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback<CartItemModel> RemoveEvent { get; set; }

    private string imageSource = "";
    private string truncatedName = "";
    private string truncatedDescription = "";
    private decimal totalPrice = 0;

    protected override void OnInitialized()
    {
        TruncateTexts();
        CalculateTotalPrice();
        imageSource = ImageEndpoint.GetImage(CartItem.Artifact.ImageId);
    }

    private void TruncateTexts()
    {
        string artifactName = CartItem.Artifact.Name;
        truncatedName = artifactName?.Length > 18 ? string.Concat(artifactName.AsSpan(0, 15), "...") : artifactName;

        string description = CartItem.Artifact.Description;
        truncatedDescription = description?.Length > 30 ? string.Concat(description.AsSpan(0, 27), "...") : description;
    }

    private async Task RemoveFromCartAsync()
    {
        await RemoveEvent.InvokeAsync(CartItem);
    }

    private void CalculateTotalPrice()
    {
        totalPrice = CartItem.Artifact.FinalPrice * CartItem.Quantity;
    }

    private void OpenDetails()
    {
        Navigation.NavigateTo($"/Artifact/{CartItem.Artifact.Id}");
    }
}