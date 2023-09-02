namespace VintageHub.Client.Models;

public class CreateArtifactModel
{
    [Required(ErrorMessageResourceName = "form_artifact_name_required", ErrorMessageResourceType = typeof(Resource))]
    [StringLength(100, ErrorMessageResourceName = "form_artifact_name_length", ErrorMessageResourceType = typeof(Resource))]
    public string Name { get; set; }

    [Required(ErrorMessageResourceName = "form_artifact_description_required", ErrorMessageResourceType = typeof(Resource))]
    [StringLength(1000, ErrorMessageResourceName = "form_artifact_description_length", ErrorMessageResourceType = typeof(Resource))]
    public string Description { get; set; }

    [Display(Name = "Image")]
    public string ImageId { get; set; } = "";

    [Required(ErrorMessageResourceName = "form_artifact_quantity_required", ErrorMessageResourceType = typeof(Resource))]
    public int Quantity { get; set; }

    [Required(ErrorMessageResourceName = "form_artifact_required", ErrorMessageResourceType = typeof(Resource))]
    [Range(0.01, double.MaxValue, ErrorMessageResourceName = "form_artifact_price_range", ErrorMessageResourceType = typeof(Resource))]
    public decimal Price { get; set; }

    [Required(ErrorMessageResourceName = "form_artifact_discount_amount_required", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "Discount Amount")]
    public decimal DiscountAmount { get; set; }

    [Required(ErrorMessageResourceName = "form_artifact_vendor_required", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "Vendor")]
    public int VendorId { get; set; }

    [Required(ErrorMessageResourceName = "form_artifact_category_required", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Required(ErrorMessageResourceName = "form_artifact_era_required", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "Era")]
    public int EraId { get; set; }

    [Required(ErrorMessageResourceName = "form_artifact_availability_required", ErrorMessageResourceType = typeof(Resource))]
    public bool Availability { get; set; }
}
