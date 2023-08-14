namespace Client.Library.Models.Cache;
public class ImageModel
{
    public string ObjectIdentifier { get; set; }
    public string Url { get; set; }

    public ImageModel(string objectId, string data)
    {
        ObjectIdentifier = objectId;
        Url = data;
    }

    public ImageModel()
    {
        
    }
}
