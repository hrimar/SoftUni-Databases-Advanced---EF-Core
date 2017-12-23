namespace P01_BillsPaymentSystem.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data.Models;
    using P01_BillsPaymentSystem.Data.EntityConfig;

    public class BillsPaymentSystemContext : DbContext
    {
        public BillsPaymentSystemContext() { }

        public BillsPaymentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
      
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

            modelBuilder.ApplyConfiguration(new CreditCardConfiguration());

            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());

            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
        }
    }
}
