namespace P01_BillsPaymentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CreditCard
    {
        public CreditCard() { }

        public CreditCard(decimal limit, DateTime expirationDate)
        {
            this.Limit = limit;
            this.ExpirationDate = expirationDate;
        }

        public int CreditCardId { get; set; }
        public decimal Limit { get; set; }
        public decimal MoneyOwed { get; set; }
        public decimal LimitLeft => Limit - MoneyOwed; // not included in the database) !!!
        //public decimal LimitLeft
        //{
        //    get { return this.Limit - this.MoneyOwed; }
        //}

        public DateTime ExpirationDate { get; set; }

        public int PaymentMethodId { get; set; } // NOT TO INCLUDE IN DB !!!
        public PaymentMethod PaymentMethod { get; set; } // just for relation and easy access

        public void Withdraw(decimal amount)
        {
            this.MoneyOwed += amount;
        }

        public void Deposit(decimal amount)
        {
            this.MoneyOwed -= amount;
        }
    }
}
