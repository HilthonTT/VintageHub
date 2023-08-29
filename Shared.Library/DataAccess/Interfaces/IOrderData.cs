namespace Shared.Library.DataAccess.Interfaces;
public interface IOrderData
{
    Task<int> DeleteOrderAsync(int id);
    Task<List<OrderDisplayModel>> GetAllOrdersAsync();
    Task<OrderDisplayModel> GetOrderByIdAsync(int id);
    Task<List<OrderDetailsDisplayModel>> GetOrderDetailsByOrderIdAsync(int orderId);
    Task<List<OrderDisplayModel>> GetOrdersByUserIdAsync(int userId);
    Task<int> InsertOrderAsync(OrderModel order, List<OrderDetailsModel> orderDetails);
    Task<int> UpdateOrderAsync(OrderModel order, List<OrderDetailsModel> orderDetails);
}