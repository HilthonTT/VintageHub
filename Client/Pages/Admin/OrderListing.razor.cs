namespace VintageHub.Client.Pages.Admin;

public partial class OrderListing
{
    private List<OrderDisplayModel> orders;
    private bool isLoading = true;
    private string searchText = "";
    protected override async Task OnInitializedAsync()
    {
        orders = await OrderEndpoint.GetAllOrdersAsync();
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

    private async Task UpdateOrderAsync(OrderDisplayModel order)
    {
        var updatedOrder = new OrderModel()
        {
            Id = order.Id,
            TotalPrice = order.TotalPrice,
            DateOrdered = order.DateOrdered,
            IsCanceled = order.IsCanceled,
            IsComplete = order.IsComplete,
            UserId = order.User.Id,
        };

        var orderDetails = await OrderEndpoint.GetOrderDetailsByOrderIdAsync(updatedOrder.Id);
        var orders = new List<OrderDetailsModel>();
        foreach (var o in orderDetails)
        {
            var newOrder = new OrderDetailsModel(o);
            orders.Add(newOrder);
        }

        var request = new OrderRequestModel(updatedOrder, orders);
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