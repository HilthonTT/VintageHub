namespace Server.Library.Models;
public class ArtifactModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; }

    public string ImageId { get; set; } = "";

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Range(0, 5)]
    public double Rating { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int VendorId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int EraId { get; set; }

    [Required]
    public bool Availability { get; set; }
}
