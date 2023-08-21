using Client.Library.Models;

namespace VintageHub.Client.Models.Data;

public class OrderDetailsDataModel
{
    public OrderDetailsDataModel(OrderDetailsModel orderDetails)
    {
        OrderDetails = orderDetails;
    }

    public OrderDetailsDataModel()
    {
        
    }

    public OrderDetailsModel OrderDetails { get; set; }
    public ArtifactModel Artifact { get; set; }
    public decimal TotalPrice { get; set; }
}
