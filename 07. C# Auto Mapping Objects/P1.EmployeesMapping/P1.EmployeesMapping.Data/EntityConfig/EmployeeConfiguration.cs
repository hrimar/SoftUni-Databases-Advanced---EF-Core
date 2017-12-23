namespace P1.EmployeesMapping.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using P1.EmployeesMapping.Model;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(40);

            builder.Property(e => e.LastName)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(40);

            builder.Property(e => e.Salary)
                .IsRequired();

            builder.Property(e => e.Birthday)
                .IsRequired(false);

            builder.Property(e => e.Address)
                .IsRequired(false)
                .IsUnicode()
                .HasMaxLength(200);

            builder.Property(e => e.ManagerId)
                    .IsRequired(false);

            builder.HasOne(m=>m.Manager)
                   .WithMany(e=>e.ManagedEmployees)
                    .HasForeignKey(m=> m.ManagerId)
                    .HasConstraintName("FK_Employees_Employees");
        }
    }
}
