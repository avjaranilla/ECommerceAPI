using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Product.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        {
            return await _context.Product
            .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
            .ToListAsync();
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            return await _context.Product.Where(p => productIds.Contains(p.Id)).ToListAsync();
        }
    }
}
