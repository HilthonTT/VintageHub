namespace Server.Library.Models;
public class ReviewModel
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ArtifactId { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [Range(0, 5)]
    public int Rating { get; set; }
}
