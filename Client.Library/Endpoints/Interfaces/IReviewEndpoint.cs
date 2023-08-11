using Client.Library.Models;

namespace Client.Library.Endpoints.Interfaces;
public interface IReviewEndpoint
{
    Task DeleteReviewAsync(ReviewModel review);
    Task<List<ReviewModel>> GetAllReviewsByArtifactId(int artifactId);
    Task<ReviewModel> GetReviewByIdAsync(int id);
    Task<ReviewModel> InsertReviewAsync(ReviewModel review);
    Task UpdateReviewAsync(ReviewModel review);
}