namespace ProductsShop.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using ProductsShop.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class CategoryProductConfiguration : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            builder.HasKey(e => new { e.ProductId, e.CategoryId });

            builder.HasOne(e => e.Category)
                .WithMany(cp => cp.CategoryProducts)
                .HasForeignKey(e => e.CategoryId);

            builder.HasOne(e => e.Product)
                .WithMany(cp => cp.CategoryProducts)
                .HasForeignKey(e => e.ProductId);
        }
    }
}
