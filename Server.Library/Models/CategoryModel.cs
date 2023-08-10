namespace Server.Library.Models;
public class CategoryModel
{
    public string Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(256)]
    public string Description { get; set; }
}
