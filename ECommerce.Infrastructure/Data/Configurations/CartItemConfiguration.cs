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
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.CartItemId);

            builder.Property(ci => ci.Quantity)
                .IsRequired();

            builder.Property(ci => ci.UnitPrice)
                .IsRequired();

            builder.Property(ci => ci.TotalAmount)
                .IsRequired();

            builder.HasOne<UserCart>(ci => ci.UserCart)
                .WithMany(uc => uc.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
