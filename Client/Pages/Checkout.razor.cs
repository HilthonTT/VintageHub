namespace VintageHub.Client.Pages;

public partial class Checkout
{
    [Parameter]
    public string Guid { get; set; }

    private CreditCardPaymentModel payment = new();
    private List<string> events = new();
    private UserModel loggedInUser;
    private ShoppingCartModel shoppingCart;
    private string errorMessage = "";
    private decimal totalPrice = 0;
    private bool isLoading = true;
    private bool isAllowed = false;
    protected override async Task OnInitializedAsync()
    {
        payment.ExpirationYear = DateTime.UtcNow.Year;
        payment.ExpirationMonth = DateTime.UtcNow.Month;

        loggedInUser = await AuthProvider.GetUserFromAuth(UserEndpoint);
        shoppingCart = await ShoppingCartStorage.GetShoppingCartAsync();
        CalculatePrice();

        isAllowed = await IsAllowedAsync();
        isLoading = false;
    }

    private void CalculatePrice()
    {
        decimal price = 0;
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
            price += (decimal)shoppingCart.CartItems.First(i => i.Artifact.Id == kvp.Key).Artifact.Price * kvp.Value;
        }

        totalPrice = price;
        payment.PaymentAmount = totalPrice;
    }

    private void ClosePage()
    {
        Navigation.NavigateTo("/");
    }

    private bool CanCheckout()
    {
        if (shoppingCart?.CartItems?.Count <= 0 || loggedInUser is null)
        {
            return false;
        }

        return true;
    }

    private OrderModel GetOrder()
    {
        return new OrderModel()
        {
            UserId = loggedInUser.Id,
            TotalPrice = totalPrice,
            IsCanceled = false,
            IsComplete = false,
            DateOrdered = DateTime.UtcNow,
        };
    }

    private List<OrderDetailsModel> GetOrderDetails()
    {
        var orderDetails = new List<OrderDetailsModel>();
        foreach (var item in shoppingCart.CartItems)
        {
            var details = new OrderDetailsModel
            {
                ArtifactId = item.Artifact.Id,
                Quantity = item.Quantity,
            };
            orderDetails.Add(details);
        }

        return orderDetails;
    }

    private async Task<bool> IsAllowedAsync()
    {
        string guid = await SessionStorage.GetItemAsStringAsync("CheckoutGuid");
        return Guid == guid;
    }

    private async Task ConfirmOrderAsync()
    {
        if (CanCheckout() is false)
        {
            return;
        }

        try
        {
            errorMessage = "";
            string jsonifiedPayment = JsonSerializer.Serialize(payment);
            events.Add($"{Localizer["payment-confirmed"]} \n {jsonifiedPayment}");

            var newOrder = GetOrder();
            var orderDetails = GetOrderDetails();

            var request = new OrderRequestModel(newOrder, orderDetails);
            shoppingCart.CartItems.Clear();

            await ShoppingCartStorage.SaveShoppingCartAsync(shoppingCart);
            await OrderEndpoint.InsertOrderAsync(request);

            ClosePage();
        }
        catch
        {
            errorMessage = Localizer["credit-card-error"];
        }
    }
}