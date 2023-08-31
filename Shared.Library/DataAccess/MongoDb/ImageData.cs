namespace Shared.Library.DataAccess.MongoDb;
public class ImageData : IImageData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private readonly IMongoDbConnection _connection;
    private readonly IMemoryCache _cache;

    public ImageData(
        IMongoDbConnection connection,
        IMemoryCache cache)
    {
        _connection = connection;
        _cache = cache;
    }

    public async Task<string> UploadImageAsync(Stream imageStream)
    {
        string uniqueFileName = $"{Guid.NewGuid():N}_{DateTime.UtcNow:yyyyMMddHHmmss}.jpg";
        var objectId = await _connection.Bucket.UploadFromStreamAsync(uniqueFileName, imageStream);

        return objectId.ToString();
    }

    public async Task<byte[]> GetImageAsync(string objectId)
    {
        var output = _cache.Get<byte[]>(objectId);
        if (output is null)
        {
            var id = new ObjectId(objectId);
            output = await _connection.Bucket.DownloadAsBytesAsync(id);

            _cache.Set(objectId, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task DeleteImageAsync(string objectId)
    {
        _cache.Remove(objectId);

        var id = new ObjectId(objectId);
        await _connection.Bucket.DeleteAsync(id);
    }
}
