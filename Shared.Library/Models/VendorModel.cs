namespace Shared.Library.Models;
public class VendorModel
{
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
