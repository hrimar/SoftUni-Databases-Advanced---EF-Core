namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {

            builder.HasKey(e => e.BankAcountId);

            builder.Property(e => e.BankName)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Balance)
                        .IsRequired(); // щом не е стринг това не е задължително

            builder.Property(e => e.SwiftCode)
                         .IsUnicode(false)
                        .IsRequired()
                        .HasMaxLength(20);

            builder.Ignore(e => e.PaymentMethodId);

            //builder.Ignore(e => e.PaymentMethod);
        }
    }
}
