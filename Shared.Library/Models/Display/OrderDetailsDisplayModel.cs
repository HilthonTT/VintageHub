namespace Shared.Library.Models.Display;
public class OrderDetailsDisplayModel
{
    public int Id { get; set; }
    public OrderModel Order { get; set; }
    public ArtifactModel Artifact { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => GetTotalPrice();

    private decimal GetTotalPrice()
    {
        return Artifact.Price * Quantity;
    }
}
