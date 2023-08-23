using System.ComponentModel.DataAnnotations;

namespace VintageHub.Client.Models;

public class CreateVendorModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Please provide a value user for the owner.")]
    public int OwnerUserId { get; set; }

    [Required(ErrorMessage = "Please provide the vendor's name.")]
    [StringLength(100, ErrorMessage = "The vendor's name must not be above 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please provide the vendor's description.")]
    [StringLength(1000, ErrorMessage = "The vendor's description must not be above 1000 characters.")]
    public string Description { get; set; }

    [Required]
    [Display(Name = "Date Founded")]
    public DateTime? DateFounded { get; set; } = DateTime.Now;
}
