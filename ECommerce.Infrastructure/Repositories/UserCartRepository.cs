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
    public class UserCartRepository : IUserCartRepository
    {
        private readonly ECommerceDbContext _context;

        public UserCartRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<UserCart> AddAsync(int userId)
        {
            var newCart = new UserCart
            {
                UserId = userId,
                Status = "Pending", // Default status
            };

            _context.UserCart.Add(newCart);
            await _context.SaveChangesAsync();

            return newCart;
        }

        public async Task<UserCart> GetCartByIdAsync(int cartId)
        {
            return await _context.UserCart
           .Include(cart => cart.CartItems) // Include related cart items
           .FirstOrDefaultAsync(cart => cart.CartId == cartId);
        }

        public async Task<IEnumerable<UserCart>> GetCartByUserIdAsync(int userId)
        {
            return await _context.UserCart
            .Where(cart => cart.UserId == userId)
            .Include(cart => cart.CartItems) // Include related cart items
            .ToListAsync();
        }

        public async Task RemoveAsync(int cartId)
        {
            var cart = await _context.UserCart
            .Include(c => c.CartItems) // Make sure to load the related items
            .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart != null)
            {
                _context.UserCart.Remove(cart);
                _context.CartItem.RemoveRange(cart.CartItems); // Remove related items as well
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserCart> UpdateStatus(int cartId, string status)
        {
            var cart = await _context.UserCart.FindAsync(cartId);
            if (cart != null)
            {
                cart.Status = status;
                await _context.SaveChangesAsync();
            }
            return cart;
        }
    }
}
