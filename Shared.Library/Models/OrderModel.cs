namespace Shared.Library.Models;
public class OrderModel
{
    public OrderModel()
    {
        
    }

    public OrderModel(OrderDisplayModel order)
    {
        Id = order.Id;
        UserId = order.User.Id;
        TotalPrice = order.TotalPrice;
        IsComplete = order.IsComplete;
        IsCanceled = order.IsCanceled;
        DateOrdered = order.DateOrdered;
    }

    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    public bool IsComplete { get; set; } = false;

    [Required]
    public bool IsCanceled { get; set; } = false;
    public DateTime DateOrdered { get; set; } = DateTime.UtcNow;
}
