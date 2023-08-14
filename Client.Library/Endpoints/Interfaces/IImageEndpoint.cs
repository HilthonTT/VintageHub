using Microsoft.AspNetCore.Components.Forms;

namespace Client.Library.Endpoints.Interfaces;
public interface IImageEndpoint
{
    Task DeleteImageAsync(string objectId);
    Task<byte[]> GetImageAsync(string objectId);
    Task<string> UploadImageAsync(IBrowserFile imageFile);
}