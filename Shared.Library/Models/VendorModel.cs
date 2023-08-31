namespace Shared.Library.Models;
public class VendorModel
{
    public VendorModel()
    {
        
    }

    public VendorModel(VendorDisplayModel vendor)
    {
        Id = vendor.Id;
        OwnerUserId = vendor.Owner.Id;
        Name = vendor.Name;
        Description = vendor.Description;
        ImageId = vendor.ImageId;
        DateFounded = vendor.DateFounded;
    }

    public int Id { get; set; }

    [Required]
    public int OwnerUserId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }
    public string ImageId { get; set; } = "";

    [Required]
    public DateTime DateFounded { get; set; }
}
