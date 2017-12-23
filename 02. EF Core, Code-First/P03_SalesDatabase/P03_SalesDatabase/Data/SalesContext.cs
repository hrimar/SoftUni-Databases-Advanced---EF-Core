using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;

namespace P03_SalesDatabase
{
    public class SalesContext : DbContext
    {
        public SalesContext()    {    }

        public SalesContext(DbContextOptions options)
            :base(options) {   }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(true)
                    .HasMaxLength(50);

                entity.Property(e => e.Quantity)
                   .IsRequired();

                entity.Property(e => e.Price)
                   .IsRequired();

                entity.Property(e => e.Description)
                      .HasMaxLength(250)
                      .HasDefaultValue("No description");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.StoreId);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .IsUnicode()
                      .HasMaxLength(80);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.SaleId);

                entity.Property(e => e.Date)
                      .IsRequired()
                .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(p => p.ProductId);
                //.HasConstraintName("FK_Sales_Product");

                entity.HasOne(e => e.Store)
                   .WithMany(p => p.Sales)
                   .HasForeignKey(p => p.StoreId);
                //.HasConstraintName("FK_Sales_Store");

                entity.HasOne(e => e.Customer)
                   .WithMany(p => p.Sales)
                   .HasForeignKey(p => p.CustomerId);
                   //.HasConstraintName("FK_Sales_Customer");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(true)
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                   .IsRequired()
                   .IsUnicode(false)
                    .HasMaxLength(80);

                entity.Property(e => e.CreditCardNumber)
                   .IsUnicode(false); //IsRequired(); Б
            });
        }
    }
}
