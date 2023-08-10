using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Server.Library.DataAccess.Interfaces;
using VintageHub.Server.Library.DataAccess.Internal.Interfaces;
using VintageHub.Server.Library.Models;

namespace Server.Library.DataAccess;
public class OrderData : IOrderData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(OrderData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameUserPrefix = $"{CacheName}_User_";
    private readonly ISqlDataAccess _sql;
    private readonly IMemoryCache _cache;
    private readonly ILogger<OrderData> _logger;

    public OrderData(
        ISqlDataAccess sql,
        IMemoryCache cache,
        ILogger<OrderData> logger)
    {
        _sql = sql;
        _cache = cache;
        _logger = logger;
    }

    private static string GetOrderStoredProcedure(string operation)
    {
        return $"dbo.spOrder_{operation}";
    }

    private static string GetOrderDetailsStoredProcedure(string operation)
    {
        return $"dbo.spOrderDetails_Insert";
    }

    private static object GetOrderInsertParams(OrderModel order)
    {
        return new
        {
            order.UserId,
            order.TotalPrice,
            order.IsComplete,
            order.IsCanceled,
            order.DateOrdered,
        };
    }

    private static object GetOrderDetailsInsertParams(OrderDetailsModel orderDetails)
    {
        return new
        {
            orderDetails.OrderId,
            orderDetails.ArtifactId,
            orderDetails.Quantity,
        };
    }

    private static object GetOrderUpdateParams(OrderModel order)
    {
        return new
        {
            order.Id,
            order.TotalPrice,
            order.IsComplete,
            order.IsCanceled,
        };
    }

    private static object GetOrderDetailsUpdateParams(OrderDetailsModel orderDetails)
    {
        return new
        {
            orderDetails.Id,
            orderDetails.Quantity,
        };
    }

    private void RemoveOrderCache(OrderModel order)
    {
        string idkey = CacheNamePrefix + order.Id;
        _cache.Remove(idkey);
    }

    public async Task<List<OrderModel>> GetAllOrdersAsync()
    {
        var output = _cache.Get<List<OrderModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetOrderStoredProcedure("GetAll");
            object parameters = new { };

            output = await _sql.LoadDataAsync<OrderModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<OrderModel>> GetOrdersByUserIdAsync(int userId)
    {
        string key = CacheNameUserPrefix + userId;
        var output = _cache.Get<List<OrderModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetOrderStoredProcedure("GetByUserId");
            object parameters = new { UserId = userId };

            output = await _sql.LoadDataAsync<OrderModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<OrderModel> GetOrderByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<OrderModel>(key);
        if (output is null)
        {
            string storedProcedure = GetOrderStoredProcedure("GetById");
            object parameters = new { Id = id };

            output = await _sql.LoadFirstOrDefaultAsync<OrderModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertOrderAsync(OrderModel order, List<OrderDetailsModel> orderDetails)
    {
        try
        {
            _sql.StartTransaction();

            string orderSp = GetOrderStoredProcedure("Insert");
            object orderParameters = GetOrderInsertParams(order);
            int orderId = await _sql.SaveDataInTransactionAsync(orderSp, orderParameters);

            string orderDetailsSp = GetOrderDetailsStoredProcedure("Insert");
            foreach (var item in orderDetails)
            {
                item.OrderId = orderId;

                object parameters = GetOrderDetailsInsertParams(item);

                await _sql.SaveDataInTransactionAsync(orderDetailsSp, parameters);
            }

            _sql.CommitTransaction();

            return orderId;
        }
        catch (Exception ex)
        {
            _sql.RollbackTransaction();
            _logger.LogError("Error while inserting order: {error}", ex.Message);
            throw;
        }
    }

    public async Task<int> UpdateOrderAsync(OrderModel order, List<OrderDetailsModel> orderDetails)
    {
        try
        {
            _sql.StartTransaction();

            string orderSp = GetOrderStoredProcedure("Update");
            object orderParameters = GetOrderUpdateParams(order);
            await _sql.SaveDataInTransactionAsync(orderSp, orderParameters);

            string orderDetailsSp = GetOrderDetailsStoredProcedure("Update");
            foreach (var item in orderDetails)
            {
                object parameters = GetOrderDetailsUpdateParams(item);

                await _sql.SaveDataInTransactionAsync(orderDetailsSp, parameters);
            }

            _sql.CommitTransaction();

            RemoveOrderCache(order);

            return order.Id;
        }
        catch (Exception ex)
        {
            _sql.RollbackTransaction();
            _logger.LogError("Error while updating order: {error}", ex.Message);
            throw;
        }
    }

    public async Task<int> DeleteOrderAsync(OrderModel order)
    {
        RemoveOrderCache(order);

        string storedProcedure = GetOrderStoredProcedure("Delete");
        object parameters = new { order.Id };

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
