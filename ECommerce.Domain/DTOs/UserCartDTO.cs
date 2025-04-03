using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.DTOs
{
    public class UserCartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Ordered"
        public decimal TotalAmount { get; set; }
        public DateTime DateCreated { get; set; }
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }
}
