namespace Shared.Library.Models.Display;
public class OrderDisplayModel
{
    public int Id { get; set; }
    public UserModel User { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsComplete { get; set; }
    public bool IsCanceled { get; set; }
    public DateTime DateOrdered { get; set; }
}
