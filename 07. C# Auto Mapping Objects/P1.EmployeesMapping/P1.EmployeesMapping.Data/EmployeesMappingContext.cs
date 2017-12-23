namespace P1.EmployeesMapping.Data
{
    using System;
    using P1.EmployeesMapping.Model;
    using Microsoft.EntityFrameworkCore;
    using P1.EmployeesMapping.Data.EntityConfig;

    public class EmployeesMappingContext : DbContext

    {
        public EmployeesMappingContext() { }
        public EmployeesMappingContext(DbContextOptions options) 
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }        


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if ( !optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());                
        }
    }
}
