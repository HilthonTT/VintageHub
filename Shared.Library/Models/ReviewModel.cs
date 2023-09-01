namespace Shared.Library.Models;
public class ReviewModel
{
    public ReviewModel()
    {
        
    }

    public ReviewModel(ReviewDisplayModel review)
    {
        Id = review.Id;
        UserId = review.User.Id;
        ArtifactId = review.Artifact.Id;
        Title = review.Title;
        Description = review.Description;
        Rating = review.Rating;
    }

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
