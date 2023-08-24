namespace VintageHub.Client.Models;

public class UpdateOrderModel
{
    public bool IsComplete { get; set; }
    public bool IsCanceled { get; set; }

    public UpdateOrderModel()
    {

    }

    public UpdateOrderModel(OrderModel order)
    {
        IsComplete = order.IsComplete;
        IsCanceled = order.IsCanceled;
    }
}
