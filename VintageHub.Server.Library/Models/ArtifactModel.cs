using System.ComponentModel.DataAnnotations;

namespace VintageHub.Server.Library.Models;
public class ArtifactModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(256)]
    public string Description { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public double Rating { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int EraId { get; set; }

    [Required]
    public bool Availability { get; set; }
}
