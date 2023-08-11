namespace Client.Library.Models;
public class OrderModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsComplete { get; set; } = false;
    public bool IsCanceled { get; set; } = false;
    public DateTime DateOrdered { get; set; }
}
