using Server.Library.Models;

namespace VintageHub.Server.Models;

public class OrderRequestModel
{
    public OrderModel Order { get; set; }
    public List<OrderDetailsModel> OrderDetails { get; set; }
}
