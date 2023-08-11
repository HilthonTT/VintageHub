namespace Client.Library.Models;
public class OrderRequestModel
{
    public OrderModel Order { get; set; }
    public List<OrderDetailsModel> OrderDetails { get; set; }
}
