namespace VintageHub.Client.Pages;

public partial class PreviousOrders
{
    private UserModel loggedInUser;
    private ObservableCollection<OrderDisplayModel> orders;
    private string searchText = "";
    private bool isLoading = true;
    protected override async Task OnInitializedAsync()
    {
        loggedInUser = await AuthProvider.GetUserFromAuth(UserEndpoint);
        await LoadOrdersAsync();
        isLoading = false;
    }

    private void ClosePage()
    {
        Navigation.NavigateTo("/");
    }

    private void LoadOrderDetailsPage(OrderDisplayModel order)
    {
        Navigation.NavigateTo($"/Order/Details/{order.Id}");
    }

    private async Task LoadOrdersAsync()
    {
        if (loggedInUser is null)
        {
            return;
        }

        var orderList = await OrderEndpoint.GetOrdersByUserIdAsync(loggedInUser.Id);
        orders = new(orderList);
    }

    private async Task UpdateOrderAsync(OrderDisplayModel order)
    {
        orders.Remove(order);
        var updatedOrder = new OrderModel
        {
            Id = order.Id,
            UserId = order.User.Id,
            TotalPrice = order.TotalPrice,
            IsCanceled = order.IsCanceled,
            IsComplete = order.IsComplete,
            DateOrdered = order.DateOrdered,
        };

        var orderDetails = await OrderEndpoint.GetOrderDetailsByOrderIdAsync(updatedOrder.Id);
        var ordersList = new List<OrderDetailsModel>();
        foreach (var o in orderDetails)
        {
            var newOrder = new OrderDetailsModel(o);
            ordersList.Add(newOrder);
        }

        var request = new OrderRequestModel(updatedOrder, ordersList);
        await OrderEndpoint.UpdateOrderAsync(request);
    }

    private Func<OrderDisplayModel, bool> quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return true;
        }

        if (x.FullName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        return false;
    };
}