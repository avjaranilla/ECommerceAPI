using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Repositories
{
    public interface IOrderDetailsRepository
    {
        Task<IEnumerable<OrderDetail>> CreateAsync(List<OrderDetail> orderDetails);
        Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId);
        Task UpdateAsync(OrderDetail orderDetail);
    }
}
