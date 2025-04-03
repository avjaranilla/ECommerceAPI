using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderHeader> CreateOrderAsync(int cartId, string shippingAddress);
        Task<OrderResponseDTO> GetByOrderIdAsync(int orderId);
        Task<bool> PayOrderAsync(int orderId);

    }
}
