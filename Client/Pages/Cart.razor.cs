namespace VintageHub.Client.Pages;

public partial class Cart
{
    private UserModel loggedInUser = new();
    private ShoppingCartModel shoppingCart = new();
    private decimal totalPrice = 0;
    private int totalItems = 0;
    private bool isLoading = true;
    protected override async Task OnInitializedAsync()
    {
        loggedInUser = await AuthProvider.GetUserFromAuth(UserEndpoint);
        shoppingCart = await ShoppingCartStorage.GetShoppingCartAsync();
        CalculatePriceAndItems();
        isLoading = false;
    }

    private void CalculatePriceAndItems()
    {
        decimal price = 0;
        int itemCounts = 0;
        var uniqueArtifacts = new Dictionary<int, int>();
        foreach (var item in shoppingCart.CartItems)
        {
            if (uniqueArtifacts.ContainsKey(item.Artifact.Id)is false)
            {
                uniqueArtifacts[item.Artifact.Id] = 0;
            }

            uniqueArtifacts[item.Artifact.Id] += item.Quantity;
        }

        foreach (var kvp in uniqueArtifacts)
        {
            price += shoppingCart.CartItems.First(i => i.Artifact.Id == kvp.Key).Artifact.Price * kvp.Value;
            itemCounts += kvp.Value;
        }

        totalPrice = price;
        totalItems = itemCounts;
    }

    private List<CartItemModel> GetCombinedCartItems()
    {
        var combinedCartItems = new List<CartItemModel>();
        foreach (var groupedItem in shoppingCart.CartItems.GroupBy(item => item.Artifact.Name))
        {
            var artifact = groupedItem.First().Artifact;
            var totalQuantity = groupedItem.Sum(item => item.Quantity);
            combinedCartItems.Add(new CartItemModel { Artifact = artifact, Quantity = totalQuantity });
        }

        return combinedCartItems;
    }

    private bool CanCheckout()
    {
        if (shoppingCart?.CartItems?.Count <= 0 || loggedInUser is null)
        {
            return false;
        }

        return true;
    }

    private async Task RemoveFromCartAsync(CartItemModel cartItem)
    {
        var item = shoppingCart.CartItems.FirstOrDefault(
            c => c.Artifact.Id == cartItem.Artifact.Id && 
            c.Quantity == cartItem.Quantity);

        shoppingCart.CartItems.Remove(item);
        await ShoppingCartStorage.SaveShoppingCartAsync(shoppingCart);
    }

    private async Task CheckoutAsync()
    {
        if (CanCheckout() is false)
        {
            return;
        }

        byte[] guidBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(guidBytes);
        }

        var guid = new Guid(guidBytes);
        string stringifiedGuid = guid.ToString();

        await SessionStorage.SetItemAsStringAsync("CheckoutGuid", stringifiedGuid);
        Navigation.NavigateTo($"/Checkout/{stringifiedGuid}");
    }
}