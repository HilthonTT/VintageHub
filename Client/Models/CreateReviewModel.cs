namespace VintageHub.Client.Models;

public class CreateReviewModel
{
    [Required(ErrorMessageResourceName = "form_review_title_required", ErrorMessageResourceType = typeof(Resource))]
    [StringLength(100, ErrorMessageResourceName = "form_review_title_length", ErrorMessageResourceType = typeof(Resource))]
    public string Title { get; set; }

    [Required(ErrorMessageResourceName = "form_review_description_required", ErrorMessageResourceType = typeof(Resource))]
    [StringLength(1000, ErrorMessageResourceName = "form_review_description_length", ErrorMessageResourceType = typeof(Resource))]
    public string Description { get; set; }

    [Required(ErrorMessageResourceName = "form_review_rating_required", ErrorMessageResourceType = typeof(Resource))]
    [Range(0, 5, ErrorMessageResourceName = "form_review_rating_range", ErrorMessageResourceType = typeof(Resource))]
    public int Rating { get; set; }
}
