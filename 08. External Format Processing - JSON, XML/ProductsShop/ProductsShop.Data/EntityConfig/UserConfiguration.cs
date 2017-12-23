namespace ProductsShop.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using ProductsShop.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Age)
             .IsRequired(false);

            builder.Property(e => e.FirstName)
                        .IsRequired(false)
                        .HasMaxLength(40);

            builder.Property(e => e.LastName)
                     .IsRequired()
                     .HasMaxLength(40);
        }
    }
}
