namespace Client.Library.Models.Display;
public class ArtifactDisplayModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageId { get; set; }
    public int Quantity { get; set; }
    public double Rating { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountAmount { get; set; }
    public VendorModel Vendor { get; set; }
    public CategoryModel Category { get; set; }
    public EraModel Era { get; set; }
    public bool Availability { get; set; }
    public decimal FinalPrice { get; set; }
}
