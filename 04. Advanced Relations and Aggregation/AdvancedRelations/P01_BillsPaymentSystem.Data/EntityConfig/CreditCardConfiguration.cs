namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P01_BillsPaymentSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.HasKey(e => e.CreditCardId);

            builder.Property(e => e.Limit)
                .IsRequired();

            builder.Property(e => e.MoneyOwed)
                .IsRequired();

              builder.Ignore(e => e.LimitLeft);

            builder.Ignore(e => e.PaymentMethodId);

            //builder.Ignore(e => e.PaymentMethod);

            builder.Property(e => e.ExpirationDate)
                .IsRequired();

        }
    }
}
