namespace Shared.Library.Models;
public class OrderDetailsModel
{
    public OrderDetailsModel()
    {
        
    }

    public OrderDetailsModel(OrderDetailsDisplayModel orderDetails)
    {
        Id = orderDetails.Id;
        OrderId = orderDetails.Order.Id;
        ArtifactId = orderDetails.Artifact.Id;
        Quantity = orderDetails.Quantity;
    }

    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ArtifactId { get; set; }
    public int Quantity { get; set; }
}
