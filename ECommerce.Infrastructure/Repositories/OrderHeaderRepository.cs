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
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly ECommerceDbContext _context;

        public OrderHeaderRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<OrderHeader> CreateAsync(OrderHeader orderHeader)
        {
            await _context.OrderHeader.AddAsync(orderHeader);
            await _context.SaveChangesAsync();
            return orderHeader;
        }

        public async Task<OrderHeader> GetByIdAsync(int orderId)
        {
            var result = await _context.OrderHeader
              .Include(o => o.OrderDetails) // Include navigation properties
              .FirstOrDefaultAsync(o => o.OrderId == orderId);
            return result;
        }

        public async Task<IEnumerable<OrderHeader>> GetByUserIdAsync(int userId)
        {
            return await _context.OrderHeader
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(OrderHeader orderHeader)
        {
            _context.OrderHeader.Update(orderHeader);
            await _context.SaveChangesAsync();
        }
    }
}
