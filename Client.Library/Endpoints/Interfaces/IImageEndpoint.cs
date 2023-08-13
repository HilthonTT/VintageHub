using Microsoft.AspNetCore.Http;

namespace Client.Library.Endpoints.Interfaces;
public interface IImageEndpoint
{
    Task DeleteImageAsync(string objectId);
    Task<byte[]> GetImageAsync(string objectId);
    Task<string> UploadImageAsync(IFormFile imageFile);
}