namespace Server.Library.DataAccess;
public class ReviewData : IReviewData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(ReviewData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameArtifactPrefix = $"{CacheName}_Artifact_";
    private readonly ISqlDataAccess _sql;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ReviewData> _logger;

    public ReviewData(
        ISqlDataAccess sql,
        IMemoryCache cache,
        ILogger<ReviewData> logger)
    {
        _sql = sql;
        _cache = cache;
        _logger = logger;
    }

    private static string GetStoredProcedure(string operation)
    {
        return $"dbo.spReview_{operation}";
    }

    private static object GetInsertParams(ReviewModel review)
    {
        return new
        {
            review.UserId,
            review.ArtifactId,
            review.Title,
            review.Description,
            review.Rating,
        };
    }

    private static object GetUpdateParams(ReviewModel review)
    {
        return new
        {
            review.Id,
            review.Title,
            review.Description,
            review.Rating,
        };
    }

    private void RemoveReviewCache(int id)
    {
        string key = CacheNameArtifactPrefix + id;
        _cache.Remove(key);
    }

    public async Task<ReviewModel> GetReviewByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<ReviewModel>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetById");
            object parameters = new { Id = id };

            output = await _sql.LoadFirstOrDefaultAsync<ReviewModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<ReviewModel>> GetReviewsByArtifactIdAsync(int artifactId)
    {
        string key = CacheNameArtifactPrefix + artifactId;
        var output = _cache.Get<List<ReviewModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetByArtifactId");
            object parameters = new { ArtifactId = artifactId };

            output = await _sql.LoadDataAsync<ReviewModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertReviewAsync(ReviewModel review)
    {
        try
        {
            _sql.StartTransaction();

            string insertSp = GetStoredProcedure("Insert");
            string getAllSp = GetStoredProcedure("GetByArtifactId");

            object parameters = GetInsertParams(review);
            int reviewId = await _sql.SaveDataInTransactionAsync(insertSp, parameters);

            // Load all reviews for the associated artifact
            var reviews = await _sql.LoadDataInTransactionAsync<ReviewModel, dynamic>(
                getAllSp, new { review.ArtifactId });

            // Calculate the new average rating based on all the reviews
            double newAverageRating = reviews.Average(r => r.Rating);

            // Load the artifact to update its rating
            var artifactToUpdate = await _sql.LoadFirstOrDefaultInTransactionAsync<ArtifactModel, dynamic>(
                "dbo.spArtifact_GetById", new { Id = review.ArtifactId });

            // Save the updated artifact back to the database

            artifactToUpdate.Rating = newAverageRating;

            await _sql.SaveDataInTransactionAsync(
                "dbo.spArtifact_Update", artifactToUpdate);

            _sql.CommitTransaction();

            return review.Id;
        }
        catch (Exception ex)
        {
            _sql.RollbackTransaction();
            _logger.LogError("Error while inserting review: {error}", ex.Message);
            throw;
        }
    }

    public async Task<int> UpdateReviewAsync(ReviewModel review)
    {
        try
        {
            _sql.StartTransaction();

            string updateSp = GetStoredProcedure("Update");
            string getById = GetStoredProcedure("GetByArtifactId");
            object parameters = GetUpdateParams(review);

            await _sql.SaveDataInTransactionAsync(updateSp, parameters);

            var updatedReviews = await _sql.LoadDataInTransactionAsync<ReviewModel, dynamic>(
                getById, new { review.ArtifactId });

            // Calculate the new average rating
            double newAverageRating = updatedReviews.Average(r => r.Rating);

            // Update the artifact's rating
            var artifactToUpdate = await _sql.LoadFirstOrDefaultInTransactionAsync<ArtifactModel, dynamic>(
                "dbo.spArtifact_GetById", new { Id = review.ArtifactId });

            artifactToUpdate.Rating = newAverageRating;

            await _sql.SaveDataInTransactionAsync(
                "dbo.spArtifact_Update", artifactToUpdate);

            _sql.CommitTransaction();

            return review.Id;
        }
        catch (Exception ex)
        {
            _sql.RollbackTransaction();
            _logger.LogError("Error while updating review: {error}", ex.Message);
            throw;
        }
    }

    public async Task<int> DeleteReviewAsync(int id)
    {
        RemoveReviewCache(id);

        string storedProcedure = GetStoredProcedure("Delete");
        object parameters = new { Id = id };

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
