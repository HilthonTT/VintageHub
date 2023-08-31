namespace Shared.Library.Endpoints.Interfaces;
public interface IReviewEndpoint
{
    Task DeleteReviewAsync(ReviewModel review);
    Task<List<ReviewDisplayModel>> GetAllReviewsByArtifactId(int artifactId);
    Task<ReviewDisplayModel> GetReviewByIdAsync(int id);
    Task<ReviewDisplayModel> InsertReviewAsync(ReviewModel review);
    Task UpdateReviewAsync(ReviewModel review);
}