using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; } // Primary Key
        public int OrderId { get; set; } // Foreign Key from OrderHeader
        public int ProductId { get; set; } // Product reference
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } // Store total instead of computing

        // Navigation Property
        public OrderHeader OrderHeader { get; set; }
    }
      
}
