using Client.Library.Models;

namespace VintageHub.Client.Models.Data;

public class OrderDetailsDataModel
{
    public OrderDetailsModel OrderDetails { get; set; }
    public ArtifactModel Artifact { get; set; }
}
