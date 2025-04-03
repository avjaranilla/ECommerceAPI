using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Repositories
{
    public interface ICartItemRepository
    {  
        Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(int cartId);
        Task<IEnumerable<CartItem>> GetItemsByCartIdsAsync(List<int> cartId);
        Task<CartItem> GetItemByIdAsync(int cartItemId);
        Task<CartItem> AddAsync(CartItem cartItem);
        Task<CartItem> UpdateAsync(CartItem cartItem);
        Task RemoveAsync(int cartItemId);
    }
}
