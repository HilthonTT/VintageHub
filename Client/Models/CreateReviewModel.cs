using System.ComponentModel.DataAnnotations;

namespace VintageHub.Client.Models;

public class CreateReviewModel
{
    [Required(ErrorMessage = "Please provide the review's title.")]
    [StringLength(50, ErrorMessage = "The title mut not be above 50 characters.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Please provide the review's description.")]
    [StringLength(256, ErrorMessage = "The description must not be above 256 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "You must provide the review's rating.")]
    [Range(0, 5, ErrorMessage = "Your rating must not be above 5.")]
    public double Rating { get; set; }
}
