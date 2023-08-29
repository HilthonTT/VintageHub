namespace Shared.Library.Models.Display;
public class ReviewDisplayModel
{
    public int Id { get; set; }
    public UserModel User { get; set; }
    public ArtifactModel Artifact { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
}
