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
    public class UserCartConfiguration : IEntityTypeConfiguration<UserCart>
    {
        public void Configure(EntityTypeBuilder<UserCart> builder)
        {
            builder.HasKey(uc => uc.CartId);
            builder.Property(uc => uc.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(uc => uc.DateCreated)
                   .IsRequired();

            builder.Property(uc => uc.LastUpdated)
                   .IsRequired();

            builder.HasMany(uc => uc.CartItems)
                   .WithOne(ci => ci.UserCart)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
