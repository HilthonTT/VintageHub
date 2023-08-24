namespace Client.Library.Models;
public class ReviewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ArtifactId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
}
