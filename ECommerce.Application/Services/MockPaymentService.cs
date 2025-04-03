using ECommerce.Application.Interfaces;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class MockPaymentService : IPaymentService
    {
        private readonly IOrderHeaderRepository _orderRepository;

        public MockPaymentService(IOrderHeaderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> ProcessPaymentAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.PaymentStatus == "PAID")
            {
                return false; // Order not found or already paid
            }

            // Simulate payment processing delay
            await Task.Delay(100);

            // Update payment status
            order.PaymentStatus = "PAID";
            await _orderRepository.UpdateAsync(order);

            return true;
        }

    }
}
