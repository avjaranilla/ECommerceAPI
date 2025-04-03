using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }  // Primary Key
        public int CartId { get; set; }  // Foreign Key referencing UserCart
        public int ProductId { get; set; }  // Foreign Key referencing Product
        public int Quantity { get; set; }  // Number of units in the cart
        public decimal UnitPrice { get; set; }  // Price per unit of the product
        public decimal TotalAmount { get; private set; } // Total Amont price * qty

        // Navigation properties
        public UserCart UserCart { get; set; }


        // Method to update TotalAmount whenever Quantity or UnitPrice changes
        public void UpdateTotal()
        {
            TotalAmount = Quantity * UnitPrice;
        }
    }
}
