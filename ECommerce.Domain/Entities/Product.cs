using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;

        // Constructor for EF Core
        private Product() { }

        public Product(string name, string description, decimal price, int stockQuantity)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
        }

        //Update Method
        public void Update(string name, string description, decimal price, int stockQuantity, bool isActive)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            IsActive = isActive;
        }

        //Deactivate Status
        public void Deactivate() => IsActive = false;

        //Update Stocks Quantity
        public void ReduceStock(int quantity)
        {
            if (quantity > StockQuantity)
                throw new InvalidOperationException("Not enough stock.");
            StockQuantity -= quantity;
        }

    }
}
