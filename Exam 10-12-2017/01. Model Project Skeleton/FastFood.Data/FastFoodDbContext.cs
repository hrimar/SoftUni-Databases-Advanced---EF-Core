using FastFood.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Data
{
	public class FastFoodDbContext : DbContext
	{
		public FastFoodDbContext()
		{
		}

		public FastFoodDbContext(DbContextOptions options)
			: base(options)
		{
		}

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			if (!builder.IsConfigured)
			{
				builder.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
            builder.Entity<Position>()
                .HasAlternateKey(s => s.Name); // for Unique

            builder.Entity<Item>()
           .HasAlternateKey(s => s.Name);

            builder.Entity<Order>()
            .Ignore(s => s.TotalPrice);

            builder.Entity<OrderItem>()
       .HasKey(s => new { s.OrderId, s.ItemId }); // for mapping tabl - FK

            builder.Entity<Item>()
                .HasOne(s => s.Category)
                .WithMany(t => t.Items)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
                .HasOne(o=>o.Order)
                .WithMany(oi=>oi.OrderItems)
                .HasForeignKey(o=>o.OrderId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
             .HasOne(o => o.Item)
             .WithMany(oi => oi.OrderItems)
             .HasForeignKey(o => o.ItemId)
              .OnDelete(DeleteBehavior.Restrict);
        }
	}
}