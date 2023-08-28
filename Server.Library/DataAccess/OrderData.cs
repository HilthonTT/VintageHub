namespace Server.Library.DataAccess;
public class OrderData : IOrderData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(OrderData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameUserPrefix = $"{CacheName}_User_";
    private const string CacheNameDetailsPrefix = $"{CacheName}_Details_";
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
        return $"dbo.spOrderDetails_{operation}";
    }

    private static DynamicParameters GetOrderIdParameters(int orderId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("OrderId", orderId);

        return parameters;
    }

    private static DynamicParameters GetUserIdParameters(int userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);

        return parameters;
    }

    private static DynamicParameters GetOrderInsertParams(OrderModel order)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", order.UserId);
        parameters.Add("TotalPrice", order.TotalPrice);
        parameters.Add("IsComplete", order.IsComplete);
        parameters.Add("IsCanceled", order.IsCanceled);
        parameters.Add("DateOrdered", order.DateOrdered);

        return parameters;
    }

    private static DynamicParameters GetOrderDetailsInsertParams(OrderDetailsModel orderDetails)
    {
        var parameters = new DynamicParameters();
        parameters.Add("OrderId", orderDetails.OrderId);
        parameters.Add("ArtifactId", orderDetails.ArtifactId);
        parameters.Add("Quantity", orderDetails.Quantity);

        return parameters;
    }

    private static DynamicParameters GetOrderUpdateParams(OrderModel order)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", order.Id);
        parameters.Add("TotalPrice", order.TotalPrice);
        parameters.Add("IsComplete", order.IsComplete);
        parameters.Add("IsCanceled", order.IsCanceled);

        return parameters;
    }

    private static DynamicParameters GetOrderDetailsUpdateParams(OrderDetailsModel orderDetails)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", orderDetails.Id);
        parameters.Add("TotalPrice", orderDetails.Quantity);

        return parameters;
    }

    private void RemoveOrderCache(int id)
    {
        string idkey = CacheNamePrefix + id;
        _cache.Remove(idkey);
    }

    private async Task UpdateArtifactQuantitiesAsync(List<OrderDetailsModel> orderDetails)
    {
        foreach (var item in orderDetails)
        {
            var artifact = await _sql.LoadFirstOrDefaultInTransactionAsync<ArtifactModel>(
                "dbo.spArtifact_GetById", ParameterHelper.GetIdParameters(item.ArtifactId));

            if (artifact is not null)
            {
                artifact.Quantity -= item.Quantity;

                if (artifact.Quantity < 0)
                {
                    artifact.Quantity = 0;
                }

                await _sql.SaveDataInTransactionAsync(
                    "dbo.spArtifact_Update", ParameterHelper.GetArtifactUpdateParameters(artifact));
            }
        }
    }

    private async Task<decimal> CalculatePriceAsync(List<OrderDetailsModel> orderDetails)
    {
        decimal price = 0;

        var uniqueArtifacts = new Dictionary<int, int>();

        foreach (var item in orderDetails)
        {
            if (uniqueArtifacts.ContainsKey(item.ArtifactId) is false)
            {
                uniqueArtifacts[item.ArtifactId] = 0;
            }

            uniqueArtifacts[item.ArtifactId] += item.Quantity;
        }

        foreach (var kvp in uniqueArtifacts)
        {
            var artifact = await _sql.LoadFirstOrDefaultInTransactionAsync<ArtifactModel>("dbo.spGetById",
                ParameterHelper.GetIdParameters(kvp.Value));

            price += artifact.Price * kvp.Value;
        }

        return price;
    }

    private async Task<bool> OrderNotPossibleAsync(List<OrderDetailsModel> orderDetails)
    {
        foreach (var o in orderDetails)
        {
            var artifact = await _sql.LoadFirstOrDefaultAsync<ArtifactModel>(
                "dbo.spArtifact_GetById", ParameterHelper.GetIdParameters(o.ArtifactId));

            if (o.Quantity > artifact.Quantity)
            {
                _logger.LogError("Error while inserting order: {error}",
                    "Order details has more quantity of artifact than the artifact has.");
                return true;
            }
        }

        return false;
    }

    public async Task<List<OrderDisplayModel>> GetAllOrdersAsync()
    {
        var output = _cache.Get<List<OrderDisplayModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetOrderStoredProcedure("GetAllDetailed");
            var parameters = new DynamicParameters();

            var user = new UserModel();

            output = await _sql.LoadDetailedDataAsync<OrderDisplayModel>(
                "Id", storedProcedure, parameters, user);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<OrderDisplayModel>> GetOrdersByUserIdAsync(int userId)
    {
        string key = CacheNameUserPrefix + userId;
        var output = _cache.Get<List<OrderDisplayModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetOrderStoredProcedure("GetByUserIdDetailed");
            var parameters = GetUserIdParameters(userId);

            var user = new UserModel();

            output = await _sql.LoadDetailedDataAsync<OrderDisplayModel>(
                "Id", storedProcedure, parameters, user);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<OrderDetailsModel>> GetOrderDetailsByOrderIdAsync(int orderId)
    {
        string key = CacheNameDetailsPrefix + orderId;
        var output = _cache.Get<List<OrderDetailsModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetOrderDetailsStoredProcedure("GetByOrderId");
            var parameters = GetOrderIdParameters(orderId);

            output = await _sql.LoadDataAsync<OrderDetailsModel>(storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<OrderDisplayModel> GetOrderByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<OrderDisplayModel>(key);
        if (output is null)
        {
            string storedProcedure = GetOrderStoredProcedure("GetByIdDetailed");
            var parameters = ParameterHelper.GetIdParameters(id);

            var user = new UserModel();

            output = await _sql.LoadFirstOrDefaultDetailedDataAsync<OrderDisplayModel>(
                "Id", storedProcedure, parameters, user);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertOrderAsync(OrderModel order, List<OrderDetailsModel> orderDetails)
    {
        try
        {
            if (await OrderNotPossibleAsync(orderDetails))
            {
                return 0;
            }

            _sql.StartTransaction();

            // Verify total price
            order.TotalPrice = await CalculatePriceAsync(orderDetails);

            string orderSp = GetOrderStoredProcedure("Insert");
            var orderParameters = GetOrderInsertParams(order);
            int orderId = await _sql.SaveDataInTransactionAsync(orderSp, orderParameters);

            string orderDetailsSp = GetOrderDetailsStoredProcedure("Insert");
            foreach (var item in orderDetails)
            {
                item.OrderId = orderId;
                var parameters = GetOrderDetailsInsertParams(item);

                await _sql.SaveDataInTransactionAsync(orderDetailsSp, parameters);
            }

            await UpdateArtifactQuantitiesAsync(orderDetails);

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
            if (await OrderNotPossibleAsync(orderDetails))
            {
                return 0;
            }

            _sql.StartTransaction();

            // Verify total price
            order.TotalPrice = await CalculatePriceAsync(orderDetails);

            string orderSp = GetOrderStoredProcedure("Update");
            var orderParameters = GetOrderUpdateParams(order);
            await _sql.SaveDataInTransactionAsync(orderSp, orderParameters);

            string orderDetailsSp = GetOrderDetailsStoredProcedure("Update");
            foreach (var item in orderDetails)
            {
                var parameters = GetOrderDetailsUpdateParams(item);
                await _sql.SaveDataInTransactionAsync(orderDetailsSp, parameters);
            }

            decimal totalPrice = 0;
            foreach (var item in orderDetails)
            {
                var artifact = await _sql.LoadFirstOrDefaultInTransactionAsync<ArtifactModel>
                    ("dbo.spArtifact_GetById", ParameterHelper.GetIdParameters(item.ArtifactId));
                totalPrice += artifact.Price * item.Quantity;
            }

            order.TotalPrice = totalPrice;

            var orderParams = GetOrderUpdateParams(order);

            await _sql.SaveDataInTransactionAsync("dbo.spOrder_Update", orderParams);

            _sql.CommitTransaction();

            RemoveOrderCache(order.Id);

            return order.Id;
        }
        catch (Exception ex)
        {
            _sql.RollbackTransaction();
            _logger.LogError("Error while updating order: {error}", ex.Message);
            throw;
        }
    }

    public async Task<int> DeleteOrderAsync(int id)
    {
        RemoveOrderCache(id);

        string storedProcedure = GetOrderStoredProcedure("Delete");
        var parameters = ParameterHelper.GetIdParameters(id);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
