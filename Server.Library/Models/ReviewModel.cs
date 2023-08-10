using System.ComponentModel.DataAnnotations;

namespace VintageHub.Server.Library.Models;
public class ReviewModel
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ArtifactId { get; set; }

    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [Required]
    [StringLength(256)]
    public string Description { get; set; }

    [Required]
    [Range(0, 5)]
    public double Rating { get; set; }
}
