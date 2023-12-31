﻿namespace VintageHub.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class OrderController : ControllerBase
{
    private readonly IOrderData _orderData;
    private readonly ILogger<OrderController> _logger;

    public OrderController(
        IOrderData orderData,
        ILogger<OrderController> logger)
    {
        _orderData = orderData;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<List<OrderDisplayModel>>> GetAllOrdersAsync()
    {
        try
        {
            var orders = await _orderData.GetAllOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the orders: {error}", ex.Message);
            return StatusCode(500, "Error fetching the orders");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<OrderDisplayModel>>> GetOrdersByUserIdAsync(int userId)
    {
        try
        {
            var orders = await _orderData.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the orders by user Id: {error}", ex.Message);
            return StatusCode(500, $"Error fetching the orders by user id of {userId}.");
        }
    }

    [HttpGet("orderDetails/{orderId}")]
    public async Task<ActionResult<List<OrderDetailsDisplayModel>>> GetOrderDetailsByOrderIdAsync(int orderId)
    {
        try
        {
            var orderDetails = await _orderData.GetOrderDetailsByOrderIdAsync(orderId);
            return Ok(orderDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the order details by order Id: {error}", ex.Message);
            return StatusCode(500, $"Error fetching the order details by order id of {orderId}.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDisplayModel>> GetOrderByIdAsync(int id)
    {
        try
        {
            var order = await _orderData.GetOrderByIdAsync(id);

            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the order by Id: {error}", ex.Message);
            return StatusCode(500, $"Error fetching the order by id of {id}.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<OrderDisplayModel>> InsertOrderAsync([FromBody] OrderRequestModel request)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            int createdOrderId = await _orderData.InsertOrderAsync(request.Order, request.OrderDetails);
            var createdOrder = await _orderData.GetOrderByIdAsync(createdOrderId);

            return Ok(createdOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inserting the order: {error}", ex.Message);
            return StatusCode(500, $"Error inserting the order.");
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateOrderAsync([FromBody] OrderRequestModel request)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _orderData.UpdateOrderAsync(request.Order, request.OrderDetails);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating the order: {error}", ex.Message);
            return StatusCode(500, $"Error updating the order.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrderAsync(int id)
    {
        try
        {
            await _orderData.DeleteOrderAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting the order: {error}", ex.Message);
            return StatusCode(500, $"Error deleting the order.");
        }
    }
}
