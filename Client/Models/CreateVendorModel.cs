namespace VintageHub.Client.Models;

public class CreateVendorModel
{
    [Range(1, int.MaxValue, ErrorMessageResourceName = "form_vendor_owner_required", ErrorMessageResourceType = typeof(Resource))]
    public int OwnerUserId { get; set; }

    [Required(ErrorMessageResourceName = "form_vendor_name_required", ErrorMessageResourceType = typeof(Resource))]
    [StringLength(100, ErrorMessageResourceName = "form_vendor_name_length", ErrorMessageResourceType = typeof(Resource))]
    public string Name { get; set; }

    [Required(ErrorMessageResourceName = "form_vendor_description_required", ErrorMessageResourceType = typeof(Resource))]
    [StringLength(1000, ErrorMessageResourceName = "form_vendor_description_length", ErrorMessageResourceType = typeof(Resource))]
    public string Description { get; set; }

    [Required(ErrorMessageResourceName = "form_vendor_date_founded_required", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "Date Founded")]
    public DateTime? DateFounded { get; set; } = DateTime.Now;
}
