namespace ProductsShop.Data
{
    using Microsoft.EntityFrameworkCore;
    using ProductsShop.Data.EntityConfig;
    using ProductsShop.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ProductsShopContext  : DbContext
    {
        public ProductsShopContext() {  }
        public ProductsShopContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {          
            if (!optionBuilder.IsConfigured)
            {
                optionBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            modelBuilder.ApplyConfiguration(new CategoryProductConfiguration());

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}
