namespace ProductsShop.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using ProductsShop.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(240);

            builder.Property(e => e.BuyerId)
                    .IsRequired(false);

            builder.HasOne(u => u.Buyer)
                    .WithMany(p => p.BuyedProducts)
                    .HasForeignKey(u => u.BuyerId);

            builder.HasOne(u => u.Seller)
                  .WithMany(p => p.SelledProducts)
                  .HasForeignKey(u => u.SellerId);
        }
    }
}
