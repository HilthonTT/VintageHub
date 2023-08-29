namespace Shared.Library.Models;
public class OrderModel
{
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
