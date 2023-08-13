using Microsoft.AspNetCore.Http;
using Server.Library.DataAccess.MongoDb.Interfaces;

namespace Server.Library.DataAccess.MongoDb;
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

    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        using var memoryStream = new MemoryStream();
        await imageFile.CopyToAsync(memoryStream);

        string uniqueFileName = $"{Guid.NewGuid():N}_{DateTime.UtcNow:yyyyMMddHHmmss}.jpg";
        var objectId = await _connection.Bucket.UploadFromStreamAsync(uniqueFileName, memoryStream);

        return objectId.ToString();
    }

    public async Task<byte[]> GetImageAsync(string objectId)
    {
        var output = _cache.Get<byte[]>(objectId);
        if (output is null)
        {
            output = await _connection.Bucket.DownloadAsBytesAsync(objectId);
            _cache.Set(objectId, output, CacheTimeSpan);
        }

        return output;
    }
}
