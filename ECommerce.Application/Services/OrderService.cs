using ECommerce.Application.Interfaces;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUserCartRepository _cartRepository;
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public OrderService(IUserCartRepository cartRepository, IOrderHeaderRepository orderHeaderRepository,
            IOrderDetailsRepository orderDetailsRepository, ICartItemRepository cartItemRepository)
        {
            _cartRepository = cartRepository;
            _orderHeaderRepository = orderHeaderRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<OrderResponseDTO> GetByOrderIdAsync(int orderId)
        {
            // Step 1: Get the OrderHeader by OrderId
            var orderHeader = await _orderHeaderRepository.GetByIdAsync(orderId);

            if (orderHeader == null)
            {
                return null;
            }

            // Step 2: Map the OrderHeader and OrderDetails to OrderResponseDto
            var orderResponse = new OrderResponseDTO
            {
                OrderId = orderHeader.OrderId,
                UserId = orderHeader.UserId,
                PaymentStatus = orderHeader.PaymentStatus,
                ShippingAddress = orderHeader.ShippingAddress,
                TotalAmount = orderHeader.TotalAmount,
                OrderDate = orderHeader.OrderDate,
                OrderDetails = orderHeader.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    TotalPrice = od.TotalPrice
                }).ToList()
            };

            return orderResponse;
        }

        public async Task<OrderHeader> CreateOrderAsync(int cartId, string shippingAddress)
        {
            // Step 1: Retrieve the Cart by CartID
            var cart = await _cartRepository.GetCartByIdAsync(cartId);
            if (cart == null)
            {
                throw new ArgumentException("Invalid Cart ID.");
            }

            // Step 2: Calculate the Total Amount by summing up the TotalAmount from CartItems
            var cartItems = await _cartItemRepository.GetItemsByCartIdsAsync(new List<int> { cartId });
            decimal totalAmount = cartItems.Sum(item => item.TotalAmount);

            // Step 3: Create OrderHeader based on Cart and additional details
            var orderHeader = new OrderHeader
            {
                UserId = cart.UserId,
                PaymentStatus = "PENDING", // Default to "PENDING" as no payment status is passed
                ShippingAddress = shippingAddress,
                TotalAmount = totalAmount, // Using the summed total from CartItems
                OrderDate = DateTime.Now
            };

            // Step 4: Save the OrderHeader to the repository
            orderHeader = await _orderHeaderRepository.CreateAsync(orderHeader);

            // Step 5: Create OrderDetails by copying CartItems to OrderDetails
            var orderDetails = cartItems.Select(item => new OrderDetail
            {
                OrderId = orderHeader.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalAmount // Copy the TotalPrice directly
            }).ToList();

            // Save OrderDetails
            await _orderDetailsRepository.CreateAsync(orderDetails);

            // Step 6: Update CartStatus to "ORDERED"
            var result = await _cartRepository.UpdateStatus(cartId, "ORDERED");

            // Step 7: Return the created OrderHeader
            return orderHeader;
        }
    }
}
