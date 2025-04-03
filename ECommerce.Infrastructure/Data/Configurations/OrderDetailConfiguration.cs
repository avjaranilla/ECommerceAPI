using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => od.OrderDetailId); // Primary Key

            builder.Property(od => od.OrderId)
                   .IsRequired();

            builder.Property(od => od.ProductId)
                   .IsRequired();

            builder.Property(od => od.Quantity)
                   .IsRequired();

            builder.Property(od => od.UnitPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)"); // Ensure proper decimal precision

            builder.Property(od => od.TotalPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Relationship: OrderDetail belongs to OrderHeader
            builder.HasOne(od => od.OrderHeader)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
