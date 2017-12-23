namespace P01_BillsPaymentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BankAccount
    {
        public BankAccount() { }

        public BankAccount(decimal balance, string bankName, string swiftCode)
        {
            this.Balance = balance;
            this.BankName = bankName;
            this.SwiftCode = swiftCode;
        }

        public int BankAcountId { get; set; }
        public decimal Balance { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }

        public int PaymentMethodId { get; set; } // NOT TO INCLUDE IN DB,
        public PaymentMethod PaymentMethod { get; set; } // just for relation and easy access

        public void Withdraw(decimal amount)
        {
            this.Balance -= amount;
        }

        public void Deposit(decimal amount)
        {
            this.Balance += amount;
        }
    }
}
