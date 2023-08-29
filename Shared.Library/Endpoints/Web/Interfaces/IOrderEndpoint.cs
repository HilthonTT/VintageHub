namespace Shared.Library.Endpoints.Web.Interfaces;
public interface IOrderEndpoint
{
    Task DeleteOrderAsync(OrderModel order);
    Task<List<OrderDisplayModel>> GetAllOrdersAsync();
    Task<OrderDisplayModel> GetOrderByIdAsync(int id);
    Task<List<OrderDetailsDisplayModel>> GetOrderDetailsByOrderIdAsync(int orderId);
    Task<List<OrderDisplayModel>> GetOrdersByUserIdAsync(int userId);
    Task<OrderDisplayModel> InsertOrderAsync(OrderRequestModel request);
    Task UpdateOrderAsync(OrderRequestModel request);
}