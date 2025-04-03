using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly ECommerceDbContext _context;

        public OrderDetailsRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetail>> CreateAsync(List<OrderDetail> orderDetails)
        {
            await _context.OrderDetail.AddRangeAsync(orderDetails);
            await _context.SaveChangesAsync();
            return orderDetails;
        }

        public async Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetail
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }

        public async Task UpdateAsync(OrderDetail orderDetail)
        {
            _context.OrderDetail.Update(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
