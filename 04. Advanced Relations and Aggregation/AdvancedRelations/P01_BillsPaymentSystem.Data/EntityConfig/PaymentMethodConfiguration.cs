using System;
using System.Collections.Generic;
using System.Text;

namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(e => e.Id);

            // Само защото някои могат да са NULL затова не може та бъдат KEY
           // builder.HasAlternateKey(e => new { e.UserId, e.BankAccountId, e.CreditCardId });
            // затова в този случай:
            builder.HasIndex(e => new { e.UserId, e.BankAccountId, e.CreditCardId }).IsUnique();

            builder.HasOne(e => e.User)
                .WithMany(m => m.PaymentMethods)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.CreditCard)
               .WithOne(m => m.PaymentMethod)
               .HasForeignKey<PaymentMethod>(e => e.CreditCardId);

            builder.HasOne(e => e.BankAccount)
                    .WithOne(ba => ba.PaymentMethod)
                    .HasForeignKey<PaymentMethod>(e => e.BankAccountId);
        }                                
    }
}
