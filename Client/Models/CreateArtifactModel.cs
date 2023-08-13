using System.ComponentModel.DataAnnotations;

namespace VintageHub.Client.Models;

public class CreateArtifactModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(256)]
    public string Description { get; set; }

    public string ImageUrl { get; set; } = "";

    [Required]
    public int Quantity { get; set; }

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
