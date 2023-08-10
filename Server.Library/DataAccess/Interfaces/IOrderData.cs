namespace Server.Library.DataAccess.Interfaces;
public interface IOrderData
{
    Task<int> DeleteOrderAsync(OrderModel order);
    Task<List<OrderModel>> GetAllOrdersAsync();
    Task<OrderModel> GetOrderByIdAsync(int id);
    Task<List<OrderDetailsModel>> GetOrderDetailsByOrderIdAsync(int orderId);
    Task<List<OrderModel>> GetOrdersByUserIdAsync(int userId);
    Task<int> InsertOrderAsync(OrderModel order, List<OrderDetailsModel> orderDetails);
    Task<int> UpdateOrderAsync(OrderModel order, List<OrderDetailsModel> orderDetails);
}