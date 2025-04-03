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
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ECommerceDbContext _context;

        public CartItemRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        // Get all items for a given lsit of CartIds
        public async Task<IEnumerable<CartItem>> GetItemsByCartIdsAsync(List<int> cartIds)
        {
            return await _context.CartItem
                .Where(ci => cartIds.Contains(ci.CartId))
                .ToListAsync();
        }

        // Get all items for a given cartId
        public async Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItem
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }

        // Get a single cart item by its CartItemId
        public async Task<CartItem> GetItemByIdAsync(int cartItemId)
        {
            return await _context.CartItem
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }

        // Add a new item to a cart
        public async Task<CartItem> AddAsync(CartItem cartItem)
        {
            await _context.CartItem.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        // Update an existing cart item (quantity or price)
        public async Task<CartItem> UpdateAsync(CartItem cartItem)
        {
            var existingItem = await _context.CartItem
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItem.CartItemId);

            if (existingItem != null)
            {
                // Update fields as necessary
                existingItem.Quantity = cartItem.Quantity;
                existingItem.UnitPrice = cartItem.UnitPrice;
                existingItem.UpdateTotal();  // Recalculate total amount

                _context.CartItem.Update(existingItem);
                await _context.SaveChangesAsync();
                return existingItem;
            }

            return null; // Return null if item was not found
        }

        // Remove a cart item by its CartItemId
        public async Task RemoveAsync(int cartItemId)
        {
            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (cartItem != null)
            {
                _context.CartItem.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
