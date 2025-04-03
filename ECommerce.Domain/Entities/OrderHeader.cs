using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public  class OrderHeader
    {
        public int OrderId { get; set; }  // Primary Key
        public int UserId { get; set; }  // Foreign Key referencing the User
        public DateTime OrderDate { get; set; }  // Date of the order
        public decimal TotalAmount { get; set; }  // Total price of the order
        public string PaymentStatus { get; set; }  // Payment status (Pending, Paid)
        public string ShippingAddress { get; set; }  // Shipping address (optional)

        // Navigation Property
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
   
}
