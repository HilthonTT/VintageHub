using Client.Library.Models;

namespace Client.Library.Endpoints.Interfaces;
public interface IOrderEndpoint
{
    Task DeleteOrderAsync(OrderModel order);
    Task<List<OrderModel>> GetAllOrdersAsync();
    Task<OrderModel> GetOrderByIdAsync(int id);
    Task<List<OrderDetailsModel>> GetOrderDetailsByOrderIdAsync(int orderId);
    Task<List<OrderModel>> GetOrdersByUserIdAsync(int userId);
    Task<OrderModel> InsertOrderAsync(OrderRequestModel request);
    Task UpdateOrderAsync(OrderRequestModel request);
}