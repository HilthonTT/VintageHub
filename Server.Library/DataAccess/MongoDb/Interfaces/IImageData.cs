using Microsoft.AspNetCore.Http;

namespace Server.Library.DataAccess.MongoDb.Interfaces;
public interface IImageData
{
    Task DeleteImageAsync(string objectId);
    Task<string> GetImageAsync(string objectId);
    Task<string> UploadImageAsync(IFormFile imageFile);
}