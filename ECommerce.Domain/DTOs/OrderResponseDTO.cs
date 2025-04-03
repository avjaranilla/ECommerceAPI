using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.DTOs
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string PaymentStatus { get; set; } // e.g., "PENDING", "COMPLETED"
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();

    }
}
