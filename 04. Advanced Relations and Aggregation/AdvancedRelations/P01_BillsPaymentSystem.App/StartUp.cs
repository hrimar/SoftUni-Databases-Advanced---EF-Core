namespace P01_BillsPaymentSystem//.App
{
    using System;
    using P01_BillsPaymentSystem.Data;
    using P01_BillsPaymentSystem.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Linq;

    public class StartUp
    {
        static void Main()
        {
            using (var db = new BillsPaymentSystemContext())
            {
                //db.Database.EnsureCreated(); 
                //db.Database.Migrate();

                //// Excercise 2.	Seed Some Data:
                // Seed(db);

                // Excercise 4.	Pay Bills :
                var userId = int.Parse(Console.ReadLine());
                var amount = decimal.Parse(Console.ReadLine());
                PayBills(db, userId, amount);
            }

            //            // Excercise 3 - 3.	User Details:
            //            var userId = int.Parse(Console.ReadLine());

            //            using (var db = new BillsPaymentSystemContext())
            //            {
            //                var user = db.Users
            //                    .Where(u => u.UserId == userId)
            //                    .Select(u => new
            //                    {
            //                        Name = $"{u.FirstName} {u.LastName}",
            //                        CreditCards = u.PaymentMethods
            //                            .Where(pm => pm.Type == PaymentMethodType.CreditCard)
            //                            .Select(pm => pm.CreditCard).ToList(),      // !!!
            //                        BankAccount = u.PaymentMethods
            //                            .Where(pm => pm.Type == PaymentMethodType.BankAccount)
            //                            .Select(pm => pm.BankAccount).ToList()    // !!!
            //                    }).FirstOrDefault();

            //                Console.WriteLine($"User: {user.Name}");

            //                var bankAccounts = user.BankAccount;
            //                if (bankAccounts.Any())
            //                {
            //                    Console.WriteLine("Bank Accounts:");
            //                    foreach (var ba in bankAccounts)
            //                    {
            //                        Console.WriteLine($@"-- ID: {ba.BankAcountId}
            //--- Balance: {ba.Balance:f2}
            //--- Bank: {ba.BankName}
            //--- SWIFT: {ba.SwiftCode}");
            //                    }
            //                }

            //                var creditCards = user.CreditCards;
            //                if (creditCards.Any())
            //                {
            //                    Console.WriteLine("Credit Cards:");
            //                    foreach (var cc in creditCards)
            //                    {
            //                        Console.WriteLine($@"-- ID: {cc.CreditCardId}
            //--- Limit: {cc.Limit:f2}
            //--- Money Owed: {cc.MoneyOwed:f2}
            //--- Limit Left: {cc.LimitLeft:f2}
            //--- Expiration Date: {cc.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");
            //                    }
            //                }
            //            }
        }


        private static void Seed(BillsPaymentSystemContext db)
        {
            var users = new[]
            {
                        new User(){FirstName = "Toni", LastName="Ginosh", Email = "totev@abv.bg", Password ="123abc"},
                        new User(){FirstName = "Pero", LastName="Rakore", Email = "prop@abv.bg" , Password ="gdtds"},
                        new User(){FirstName = "Kiro", LastName="Toto",   Email = "kir@abv.bg"  , Password ="55dd44ire"}
                    };

            db.Users.AddRange(users);

            CreditCard[] creditCards = new[]
            {
                        new CreditCard()
                         {
                        ExpirationDate = DateTime.ParseExact("15.05.2019", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Limit = 5000m,
                        MoneyOwed = 50
                        },
                        new CreditCard()
                         {
                        ExpirationDate = DateTime.ParseExact("30.11.2018", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Limit = 1000m,
                        MoneyOwed = 100
                        },
                    };

            db.CreditCards.AddRange(creditCards);

            BankAccount bankAccount = new BankAccount()
            {
                Balance = 1500m,
                BankName = "ReyffeisenBank",
                SwiftCode = "REYFFBANK"
            };

            db.BankAccounts.Add(bankAccount);

            PaymentMethod[] paymentMethods = new[]
            {
                        new PaymentMethod()
                        {
                            User = users[0],
                            CreditCard = creditCards[0],
                            Type = PaymentMethodType.CreditCard
                        },
                          new PaymentMethod()
                        {
                            User = users[1],
                            CreditCard = creditCards[1],
                            Type = PaymentMethodType.CreditCard
                        },
                            new PaymentMethod()
                        {
                            User = users[0],
                            BankAccount = bankAccount,
                            Type = PaymentMethodType.BankAccount
                        }
                };
            db.PaymentMethods.AddRange(paymentMethods);

            db.SaveChanges();
        }

        public static void PayBills(BillsPaymentSystemContext db, int userId, decimal amount)
        {
            var cardsMoney = db.Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.CreditCard)
                .FirstOrDefault(u => u.UserId == userId)
                .PaymentMethods
                .Where(pm => pm.Type == PaymentMethodType.CreditCard)
                .OrderBy(pm => pm.CreditCardId).ToList();

            
            var bankAccountMoney = db.Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.CreditCard)
                .FirstOrDefault(u => u.UserId == userId)
                .PaymentMethods
                .Where(pm => pm.Type == PaymentMethodType.BankAccount)
                .OrderBy(pm => pm.BankAccountId).ToList();

            
            decimal totalMoney = bankAccountMoney.Sum(pm => pm.BankAccount.Balance)
                + cardsMoney.Sum(c => c.CreditCard.Limit);

            Console.WriteLine($"Total money: {totalMoney}");
            
            if (totalMoney >= amount)
            {
                foreach (var account in bankAccountMoney)
                {
                    decimal moneyToBeTaken = Math.Min(amount, account.BankAccount.Balance);
                    account.BankAccount.Withdraw(moneyToBeTaken);
                    amount -= moneyToBeTaken;

                    if (amount == 0)
                    {
                        break;
                    }
                }

                foreach (var card in cardsMoney)
                {
                    if (amount == 0)
                    {
                        break;
                    }

                    decimal moneyToBeTaken = Math.Min(amount, card.CreditCard.LimitLeft);
                    card.CreditCard.Withdraw(moneyToBeTaken);
                    amount -= moneyToBeTaken;

                    if (amount == 0)
                    {
                        break;
                    }
                }

                decimal moneyLeft = bankAccountMoney.Sum(pm => pm.BankAccount.Balance);
                decimal cardsLimitLeft = cardsMoney.Sum(c => c.CreditCard.LimitLeft);
                decimal cardsOwedMoney = cardsMoney.Sum(c => c.CreditCard.MoneyOwed);
       
                Console.WriteLine($"Money left: {moneyLeft}");
                Console.WriteLine($"Card limit left: {cardsLimitLeft}");
                Console.WriteLine($"Card owed money: {cardsOwedMoney}");
           
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("NotEnoughMoney");                
            }


        }


    }
}
