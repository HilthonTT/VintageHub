namespace Shared.Library.Models;
public class WishlistModel
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ArtifactId { get; set; }
}
