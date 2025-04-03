using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Repositories
{
    public interface IUserCartRepository
    {
        Task<IEnumerable<UserCart>> GetCartByUserIdAsync(int userId);
        Task<UserCart> GetCartByIdAsync(int cartId);
        Task<UserCart> AddAsync(int userId);
        Task<UserCart> UpdateStatus(int cartId, string status);
        Task RemoveAsync(int cartId);
    }
}
