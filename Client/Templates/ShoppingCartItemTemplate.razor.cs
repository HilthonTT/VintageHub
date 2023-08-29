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
    private decimal totalPrice = 0;

    protected override void OnInitialized()
    {
        CalculateTotalPrice();
        imageSource = ImageEndpoint.GetImage(CartItem.Artifact.ImageId);
    }

    private async Task RemoveFromCartAsync()
    {
        await RemoveEvent.InvokeAsync(CartItem);
    }

    private void CalculateTotalPrice()
    {
        totalPrice = CartItem.Artifact.Price * CartItem.Quantity;
    }

    private void OpenDetails()
    {
        Navigation.NavigateTo($"/Artifact/{CartItem.Artifact.Id}");
    }
}