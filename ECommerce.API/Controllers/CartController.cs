using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserIdFromJwt()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        [Authorize(Roles = "Admin,Customer")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCart([FromBody] List<ProductOrderModel> productDetails)
        {
            int userId = GetUserIdFromJwt();
            if (productDetails == null || productDetails.Count == 0)
            {
                return BadRequest("You must provide at least one product.");
            }

            try
            {
                var createdCart = await _cartService.CreateCartAsync(userId, productDetails);
                return Ok(createdCart); // Return the created cart with its items.
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating cart: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetCartByUserId()
        {
            int userId = GetUserIdFromJwt();
            var userCart = await _cartService.GetCartByUserIdAsync(userId);
            if (userCart == null)
            {
                return NotFound("No cart found for the user.");
            }
            return Ok(userCart);
        }

        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartById(int cartId)
        {
            try
            {
                var cart = await _cartService.GetCartByIdAsync(cartId);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin,Customer")]
        [HttpPost("{cartId}/upsert-items")]
        public async Task<IActionResult> UpsertItemsInCart(int cartId, [FromBody] List<ProductOrderModel> productDetails)
        {
            if (productDetails == null || productDetails.Count == 0)
            {
                return BadRequest("Cart must have at least one valid item.");
            }

            try
            {
                var updatedCart = await _cartService.UpsertItemsInTheCartAsync(cartId, productDetails);
                return Ok(updatedCart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
               // logging here
                return StatusCode(500, "An error occurred while updating the cart.");
            }
        }

    }
}
