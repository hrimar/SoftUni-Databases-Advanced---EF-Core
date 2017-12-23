namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            string input = (Console.ReadLine());

            using (var context = new BookShopContext())
            {
                 //DbInitializer.ResetDatabase(context);

                Console.WriteLine(GetBooksByAgeRestriction(context, input));

                //Console.WriteLine(GetGoldenBooks(context));

                //var result3 = GetBooksByPrice(context);// 0/100 ????????????????
                //Console.WriteLine(result3);

                //var result4 = GetBooksNotRealeasedIn(context, year);
                //Console.WriteLine(result4);

                //string result5 = GetBooksByCategory(context, input);
                //Console.WriteLine(result5);


                //string result12 = GetTotalProfitByCategory(context);
                //Console.WriteLine(result12);

                //    string result13 = GetMostRecentBooks(context); //100/100
                //    Console.WriteLine(result13);
            }
        }             

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            // Вариант 1 - парсване на стринг към Еnum:
            //var commandEnum = Enum.Parse<AgeRestriction>(command);

            // Вариант 2 - парсване на стринг към Еnum:
            var commandEnum = Enum.TryParse<AgeRestriction>(command, out var type) ? type : AgeRestriction.Minor;

            // Вариант 3:
            //int agetype = -1;
            //switch (command.ToLower())
            //{
            //    case "minor": agetype = 0; break;
            //    case "teen": agetype = 1; break;
            //    case "adult": agetype = 2; break;
            //}

            var books = context.Books
                .Where(b => b.AgeRestriction ==commandEnum)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();
            var sb = new StringBuilder();
            foreach (var b in books)
            {
                //sb.AppendLine($"{b}");
                sb.Append($"{b}" + Environment.NewLine);
            }
            return sb.ToString();
            //---------------
            //int agetype = -1;
            //switch (command.ToLower())
            //{
            //    case "minor": agetype = 0; break;
            //    case "teen": agetype = 1; break;
            //    case "adult": agetype = 2; break;
            //}

            //var books = context.Books
            //    .Where(b => (int)b.AgeRestriction == agetype)
            //    .Select(b => b.Title)
            //    .OrderBy(b => b)
            //    .ToArray();
            //var sb = new StringBuilder();
            //foreach (var b in books)
            //{
            //    //sb.AppendLine($"{b}");
            //    sb.Append($"{b}"+ Environment.NewLine);
            //}
            //return sb.ToString();

            // кратко решение!!!:
            //string[] titles = context.Books
            //        .Where(b => b.AgeRestriction == (AgeRestriction)agetype)
            //        .Select(t => t.Title)
            //        .OrderBy(t => t)
            //        .ToArray();
            //string result = String.Join(Environment.NewLine, titles);

            //return result;

            // Мое решение:
            //var books = context.Books
            //        .Where(b => (int)b.AgeRestriction == agetype)
            //        .Select(b => new
            //        {
            //            Title = b.Title
            //        }).OrderBy(b => b.Title)
            //        .ToArray();

            //var result = new List<string>();
            //foreach (var book in books)
            //{
            //    //Console.WriteLine($"{book.Title}");
            //    result.Add(book.Title);
            //}
            //Console.WriteLine(string.Join("\r\n", result));

            //return string.Join("\r\n", result);


            //---------------------
            //var authors = context.Authors
            //        .Select(a => new
            //        {
            //            FirstName = a.FirstName,
            //            LastName = a.LastName,
            //            Books = a.Books.Select(b => new
            //            {
            //                Title = b.Title,
            //                //Description = b.Description,
            //                //BookCategories = b.BookCategories,
            //                AgeRestriction = b.AgeRestriction,
            //                //Copies = b.Copies
            //            }).Where(b => b.AgeRestriction.Equals(command))
            //            .OrderBy(b => b.Title).ToList()
            //        })
            //        .ToList();

            ////            Console.WriteLine(string.Join(", ", authors.Any()));

            //foreach (var author in authors)
            //{
            //    foreach (var book in author.Books)
            //    {
            //        Console.WriteLine($"{book.Title}");
            //    }
            //}

        }

        public static string GetGoldenBooks(BookShopContext context)
        {

            var goldestBook = context.Books
                    .Where(c => c.Copies < 5000 && c.EditionType == EditionType.Gold)
                    .OrderBy(b => b.BookId)
                    .Select(g => g.Title)
                    .ToArray();

            string result = String.Join(Environment.NewLine, goldestBook);
            return result;
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksresult = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                }).OrderByDescending(b => b.Price)
                .ToList();

            var resultList = new List<string>();
            foreach (var book in booksresult)
            {
                string resultString = $"{book.Title} - ${book.Price}";
                resultList.Add(resultString);
            }

            string result = String.Join(Environment.NewLine, resultList);

            return result;
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                 .Where(b => b.ReleaseDate != null && b.ReleaseDate.Value.Year != year)
                 .OrderBy(b => b.BookId)
                 .Select(t => t.Title)
                 .ToArray();

            string result = String.Join(Environment.NewLine, books);
            return result;
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
           string[] categories = input.ToLower().Split(new [] { "\t", " ", Environment.NewLine }, 
                StringSplitOptions.RemoveEmptyEntries);
            
            string[] titles = context.Books
                     .Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                     .Select(b => b.Title)
                     .OrderBy(t => t).ToArray();
                      
            string result = String.Join(Environment.NewLine, titles);
            return result;
        }




        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Profit = c.CategoryBooks.Select(cop => cop.Book.Price * cop.Book.Copies).Sum()
                    //{
                    //    Profit = (cop.Book.Copies) * (cop.Book.Price) // не така
                    //})             
                }).ToArray().OrderByDescending(c => c.Profit);

            //var builder = new StringBuilder();

            //foreach (var item in categories)
            //{
            //    builder.Append($"{item.Name} {item.Profit}" + Environment.NewLine);                
            //}
            //return builder.ToString();

            string[] stringArrays = categories.Select(c => $"{c.Name} {c.Profit}").ToArray();
            string result = String.Join(Environment.NewLine, stringArrays);
            return result;
        }
       

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    Name = c.Name,
                    Book = c.CategoryBooks.Select(b => b.Book)
                    .OrderByDescending(b => b.ReleaseDate).Take(3)
                    //.Select(b => $"{b.Title} ({b.ReleaseDate.Value.Year})") // зна1и това може и после да се вземе ?!
                }).ToArray();

            //foreach (var c in categories)
            //{
            //    Console.WriteLine($"--{c.Name}");
            //    foreach (var b in c.Book)
            //    {
            //        Console.WriteLine($"{b}");
            //    }
            //}
            var builder = new StringBuilder();

            foreach (var c in categories)
            {
                builder.AppendLine($"--{c.Name}");
                foreach (var b in c.Book)
                {
                    string year = null;
                    if (b.ReleaseDate == null)
                    {
                        year = "N/A";
                    }
                    else
                    {
                        year = b.ReleaseDate.Value.Year.ToString();
                    }
                    builder.AppendLine($"{b.Title} ({year})");
                }
            }
            return builder.ToString();
        }
    }
}
