using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Repositories
{
    public interface IOrderHeaderRepository
    {
        Task<OrderHeader> CreateAsync(OrderHeader orderHeader);
        Task<OrderHeader> GetByIdAsync(int orderId);
        Task<IEnumerable<OrderHeader>> GetByUserIdAsync(int userId);
        Task UpdateAsync(OrderHeader orderHeader);
    }
}
