namespace Server.Library.DataAccess.Interfaces;
public interface IReviewData
{
    Task<int> DeleteReviewAsync(ReviewModel review);
    Task<List<ReviewModel>> GetReviewsByArtifactId(int artifactId);
    Task<int> InsertReviewAsync(ReviewModel review);
    Task<int> UpdateReviewAsync(ReviewModel review);
}