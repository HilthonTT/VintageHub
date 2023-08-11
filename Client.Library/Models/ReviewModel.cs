namespace Client.Library.Models;
internal class ReviewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ArtifactId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
}
