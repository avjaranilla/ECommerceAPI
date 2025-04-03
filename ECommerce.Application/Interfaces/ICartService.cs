using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartCreationResponseDTO> CreateCartAsync(int userId, List<ProductOrderModel> productDetails);
        Task<IEnumerable<UserCartDTO>> GetCartByUserIdAsync(int userId);
        Task<UserCartDTO> GetCartByIdAsync(int cartId);

        Task<UserCartDTO> UpsertItemsInTheCartAsync(int cartId, List<ProductOrderModel> productDetails);
    }

    public class ProductOrderModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
