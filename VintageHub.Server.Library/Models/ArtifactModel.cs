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
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int EraId { get; set; }
    public bool Availability { get; set; }
}
