namespace ProductsShop.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using ProductsShop.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired();

        }
    }
}
