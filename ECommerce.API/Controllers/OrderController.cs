using ECommerce.API.Models;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequestModel model)
        {
            if (model == null || model.CartId <= 0 || string.IsNullOrEmpty(model.ShippingAddress))
            {
                return BadRequest("Invalid request parameters.");
            }

            try
            {
                // Call service method to create an order
                var orderHeader = await _orderService.CreateOrderAsync(model.CartId, model.ShippingAddress);

                return CreatedAtAction(nameof(GetOrderById), new { orderId = orderHeader.OrderId }, orderHeader);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var orderHeader = await _orderService.GetByOrderIdAsync(orderId);

            if (orderHeader == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            return Ok(orderHeader);
        }

        [HttpPut("pay/{orderId}")]
        public async Task<IActionResult> PayOrder(int orderId)
        {
            var result = await _orderService.PayOrderAsync(orderId);

            if (!result)
            {
                return BadRequest("Payment failed or order not found.");
            }

            return Ok("Payment successful. Order status updated to PAID.");
        }

    }
}
