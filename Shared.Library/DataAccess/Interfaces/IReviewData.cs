namespace Shared.Library.DataAccess.Interfaces;
public interface IReviewData
{
    Task<int> DeleteReviewAsync(int id);
    Task<ReviewDisplayModel> GetReviewByIdAsync(int id);
    Task<List<ReviewDisplayModel>> GetReviewsByArtifactIdAsync(int artifactId);
    Task<int> InsertReviewAsync(ReviewModel review);
    Task<int> UpdateReviewAsync(ReviewModel review);
}