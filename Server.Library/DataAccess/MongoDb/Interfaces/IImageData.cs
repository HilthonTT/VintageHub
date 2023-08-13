using Microsoft.AspNetCore.Http;

namespace Server.Library.DataAccess.MongoDb.Interfaces;
public interface IImageData
{
    Task<byte[]> GetImageAsync(string objectId);
    Task<string> UploadImageAsync(IFormFile imageFile);
}