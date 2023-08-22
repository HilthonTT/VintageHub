namespace Client.Library.Models;
public class OrderRequestModel
{
    public OrderRequestModel(OrderModel order, List<OrderDetailsModel> orderDetails)
    {
        Order = order;
        OrderDetails = orderDetails;
    }

    public OrderRequestModel()
    {
        
    }

    public OrderModel Order { get; set; }
    public List<OrderDetailsModel> OrderDetails { get; set; }
}
