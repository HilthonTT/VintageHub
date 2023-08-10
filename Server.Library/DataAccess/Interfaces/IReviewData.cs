namespace Server.Library.DataAccess.Interfaces;
public interface IReviewData
{
    Task<int> DeleteReviewAsync(ReviewModel review);
    Task<ReviewModel> GetReviewByIdAsync(int id);
    Task<List<ReviewModel>> GetReviewsByArtifactIdAsync(int artifactId);
    Task<int> InsertReviewAsync(ReviewModel review);
    Task<int> UpdateReviewAsync(ReviewModel review);
}