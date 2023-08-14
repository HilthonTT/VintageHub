namespace Client.Library.Models;
public class VendorModel
{
    public int Id { get; set; }
    public int OwnerUserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageId { get; set; }
    public DateTime DateFounded { get; set; }
}
