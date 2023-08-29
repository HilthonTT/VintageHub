namespace VintageHub.Client.Pages;

public partial class OrderDetails
{
    [Parameter]
    public int Id { get; set; }

    private OrderDisplayModel order;
    private List<OrderDetailsDisplayModel> orderDetails;
    private List<string> events = new();
    private string searchText = "";
    private bool isLoading = true;
    private bool isAllowed = false;
    protected override async Task OnInitializedAsync()
    {
        isAllowed = await IsOwnerOrAdminAsync();
        if (isAllowed)
        {
            order = await OrderEndpoint.GetOrderByIdAsync(Id);
            orderDetails = await OrderEndpoint.GetOrderDetailsByOrderIdAsync(Id);
        }

        isLoading = false;
    }

    private void ClosePage()
    {
        Navigation.NavigateTo("/Admin/OrderListing");
    }

    private async Task<bool> IsOwnerOrAdminAsync()
    {
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        string jobTitle = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("jobTitle"))?.Value;
        var user = await AuthProvider.GetUserFromAuth(UserEndpoint);
        bool isAdmin = string.IsNullOrWhiteSpace(jobTitle)is false && jobTitle == "Admin";
        if (isAdmin || user?.Id == order?.User.Id)
        {
            return true;
        }

        return false;
    }

    private List<OrderDetailsModel> GetOrderDetailsList()
    {
        var list = new List<OrderDetailsModel>();
        foreach (var item in orderDetails)
        {
            var details = new OrderDetailsModel
            {
                Id = item.Id,
                ArtifactId = item.Artifact.Id,
                OrderId = item.Order.Id,
                Quantity = item.Quantity
            };
            list.Add(details);
        }

        return list;
    }

    private void StartedEditingItem(OrderDetailsDisplayModel item)
    {
        events.Insert(0, $"Event = StartedEditingItem, Data = {JsonSerializer.Serialize(item)}");
    }

    private void CanceledEditingItem(OrderDetailsDisplayModel item)
    {
        events.Insert(0, $"Event = CanceledEditingItem, Data = {JsonSerializer.Serialize(item)}");
    }

    private async Task CommittedItemChanges(OrderDetailsDisplayModel item)
    {
        events.Insert(0, $"Event = CommittedItemChanges, Data = {JsonSerializer.Serialize(item)}");

        var orderDetails = GetOrderDetailsList();
        var details = orderDetails.FirstOrDefault(x => x.Id == item.Id);

        orderDetails.Remove(details);
        details.Quantity = item.Quantity;
        orderDetails.Add(details);

        var newOrder = new OrderModel(order);
        var request = new OrderRequestModel(newOrder, orderDetails);
        await OrderEndpoint.UpdateOrderAsync(request);
    }

    private Func<OrderDetailsDisplayModel, bool> quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return true;
        }

        if (x.Artifact.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        return false;
    };
}