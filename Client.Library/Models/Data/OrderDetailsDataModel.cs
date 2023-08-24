namespace Client.Library.Models.Data;

public class OrderDetailsDataModel
{
    public OrderDetailsDataModel(OrderDetailsModel orderDetails)
    {
        Id = orderDetails.Id;
        OrderId = orderDetails.OrderId;
        ArtifactId = orderDetails.ArtifactId;
        Quantity = orderDetails.Quantity;
    }

    public OrderDetailsDataModel()
    {

    }

    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ArtifactId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public ArtifactModel Artifact { get; set; }
}
