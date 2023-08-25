namespace Client.Library.Models;
public class ArtifactModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageId { get; set; }
    public int Quantity { get; set; }
    public double Rating { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountAmount { get; set; }
    public int VendorId { get; set; }
    public int CategoryId { get; set; }
    public int EraId { get; set; }
    public bool Availability { get; set; }
    public decimal FinalPrice { get; set; }
}


// Add DiscountAmount and FinalPrice