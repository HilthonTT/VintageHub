namespace Client.Library.Models.Display;
public class VendorDisplayModel
{
    public int Id { get; set; }
    public UserModel Owner { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageId { get; set; }
    public DateTime DateFounded { get; set; }
}
