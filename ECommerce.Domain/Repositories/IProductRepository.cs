using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> SearchAsync(string keyword);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(int id);


        Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> productIds);
    }
}
