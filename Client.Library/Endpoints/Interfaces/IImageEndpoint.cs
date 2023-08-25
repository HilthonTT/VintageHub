namespace Client.Library.Endpoints.Interfaces;
public interface IImageEndpoint
{
    Task DeleteImageAsync(string objectId);
    string GetImage(string objectId);
    Task<string> UploadImageAsync(IBrowserFile imageFile);
}