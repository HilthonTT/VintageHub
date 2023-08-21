using Client.Library.Models;

namespace VintageHub.Client.Models.Data;

public class OrderDataModel
{
    public OrderDataModel()
    {
        
    }

    public OrderDataModel(OrderModel order)
    {
        Order = order;
    }

    public OrderModel Order { get; set; }
    public UserModel User { get; set; }

    public string UserFullName => $"{User?.FirstName} {User?.LastName} - {User?.Id}";
}
