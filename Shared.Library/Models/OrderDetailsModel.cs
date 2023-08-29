namespace Shared.Library.Models;
public class OrderDetailsModel
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ArtifactId { get; set; }
    public int Quantity { get; set; }
}
