using Client.Library.Models;

namespace Client.Library.Models.Data;

public class OrderDataModel
{
    public OrderDataModel()
    {

    }

    public OrderDataModel(OrderModel order)
    {
        Id = order.Id;
        UserId = order.UserId;
        TotalPrice = order.TotalPrice;
        IsComplete = order.IsComplete;
        IsCanceled = order.IsCanceled;
        DateOrdered = order.DateOrdered;
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsComplete { get; set; } = false;
    public bool IsCanceled { get; set; } = false;
    public DateTime DateOrdered { get; set; }
    public UserModel User { get; set; }

    public string UserFullName => $"{User?.FirstName} {User?.LastName} - {User?.Id}";
}
