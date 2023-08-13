namespace Client.Library.Models.Cache;
public class ImageModel
{
    public string ObjectIdentifier { get; set; }
    public byte[] Data { get; set; }

    public ImageModel(string objectId, byte[] data)
    {
        ObjectIdentifier = objectId;
        Data = data;
    }

    public ImageModel()
    {
        
    }
}
