using VintageHub.Server.Library.Models;

namespace Server.Library.DataAccess.Interfaces;
public interface IOrderData
{
    Task<int> DeleteOrderAsync(OrderModel order);
    Task<List<OrderModel>> GetAllOrdersAsync();
    Task<OrderModel> GetOrderByIdAsync(int id);
    Task<List<OrderModel>> GetOrdersByUserIdAsync(int userId);
    Task InsertOrderAsync(OrderModel order, List<OrderDetailsModel> orderDetails);
    Task UpdateOrderAsync(OrderModel order, List<OrderDetailsModel> orderDetails);
}