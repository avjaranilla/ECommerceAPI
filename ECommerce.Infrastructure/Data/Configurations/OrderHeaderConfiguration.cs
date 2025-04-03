using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class OrderHeaderConfiguration : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.HasKey(o => o.OrderId); // Primary Key

            builder.Property(o => o.UserId)
                   .IsRequired();

            builder.Property(o => o.OrderDate)
                   .IsRequired();

            builder.Property(o => o.PaymentStatus)
                   .HasMaxLength(50)
                   .IsRequired();

            // Relationship: OrderHeader has many OrderDetails
            builder.HasMany(o => o.OrderDetails)
                   .WithOne(od => od.OrderHeader)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete order details if order is deleted
        }
    }
}
