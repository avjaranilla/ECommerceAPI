using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class UserCart
    {
        public int CartId { get; set; }  // Primary Key
        public int UserId { get; set; }  // Foreign Key referencing User table
        public string Status { get; set; } = "Pending";  // Cart status: Pending, Ordered
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation property for Cart Items
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }



}
