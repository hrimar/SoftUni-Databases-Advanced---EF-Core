namespace ProductsShop.App
{
    using System;
    using ProductsShop.Models;
    using ProductsShop.Data;

    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Xml.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        static void Main()
        {
            ////0.
            //using (var db = new ProductsShopContext())
            //{
            //    db.Database.EnsureDeleted();
            //    db.Database.EnsureCreated();
            //}
            ////1. JSON
            //Console.WriteLine(ImportUsersFromJson());
            //Console.WriteLine(InportCategoriesFromJson());
            //Console.WriteLine(ImportProductsFromJson());
            //SetCategoryes();

            //var jsonStringUsers = File.ReadAllText("Files/users.json");
            //var objUser = JsonConvert.DeserializeObject<User>(jsonStringUsers);

            //2.1. JSON
            //GetProductsInRange();
            //2.2. JSON
            //GetSuccessfullySoldProducts();
            //2.3 JSON
            //GetCategoriesByProductsCount();
            //2.4 JSON
            //GetUsersAndProducts();

            //3.1 XML
            //Console.WriteLine(ImportUsersFromXml());
            //Console.WriteLine(ImportCategoriesFromXml());
            //Console.WriteLine(ImportProductsFromXml());

            //4.1. XML
            //GetProductsInRangeXML();
            //4.2. XML
            // GetSoldProductXML();
            //4.3. XML
            //GetCategoriesByProductsCountXML();
            //4.4. XML
            // GetUsersAndProductsXML();

        }

        //4.4. XML
        static void GetUsersAndProductsXML()
        {
            using (var db = new ProductsShopContext())
            {
                var users = db.Users
                    .Where(u => u.SelledProducts.Count() >= 1)
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        age = u.Age,
                        productCount = u.SelledProducts.Count(),
                        product = u.SelledProducts.Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    })
                    .OrderByDescending(u => u.productCount)
                    .ThenBy(u => u.lastName)
                    .ToArray();

                var usersCount = new
                {
                    Count = users.Count()
                };

                XDeclaration declaration = new XDeclaration("1.0", "utf-8", null);
                XDocument xDoc = new XDocument(declaration);

                var xmlDoc = new XDocument(new XElement("users", 
                                            new XAttribute("count", usersCount.Count))); // това е root-a

                foreach (var user in users)
                {
                    var userDetails = new XElement("user");

                    if (user.firstName != null)
                    {
                        userDetails.Add(new XAttribute("first-name", user.firstName));
                    }

                    userDetails.Add(new XAttribute("last-name", user.lastName));

                    if (user.age != null)
                    {
                        userDetails.Add(new XAttribute("age", user.age));
                    }
                    userDetails.Add(new XElement("sold-products", 
                        new XAttribute("count", user.product.Count())));

                    foreach (var p in user.product)
                    {
                        userDetails.Add(new XElement("product", 
                                    new XAttribute("name", p.name),
                                    new XAttribute("price", p.price)));
                    }

                    xmlDoc.Root.Add(userDetails);

                    string xmlString = xDoc.Declaration + Environment.NewLine + xmlDoc;
                    //string xmlString = xmlDoc.ToString();
                    File.WriteAllText("ExportedFiles/GetUsersAndProductsXML.xml", xmlString);
                }
            }
        }

        //4.3. XML
        static void GetCategoriesByProductsCountXML()
        {
            using (var db = new ProductsShopContext())
            {
                var categories = db.Categories
                    .Select(c => new
                    {
                        name = c.Name,
                        productsCount = c.CategoryProducts.Count(),
                        average = c.CategoryProducts.Average(p => p.Product.Price),
                        total = c.CategoryProducts.Sum(p => p.Product.Price)
                    })
                    .OrderByDescending(c => c.productsCount)
                    .ToArray();

                XDeclaration declaration = new XDeclaration("1.0", "utf-8", null);
                XDocument xDoc = new XDocument(declaration);

                var xmlDoc = new XDocument(new XElement("categories")); // това е root-a

                foreach (var c in categories)
                {
                   
                    xmlDoc.Root.Add(new XElement("catagory", 
                                    new XAttribute("name", c.name), //  значи май тук трябва да се провери за мин дълж!?
                                    new XElement("products-count", c.productsCount),
                                    new XElement("average-price", c.average),
                                    new XElement("total-revenue", c.total)));
                }
                string xmlString = xDoc.Declaration + Environment.NewLine + xmlDoc;
                //string xmlString = xmlDoc.ToString();
                File.WriteAllText("ExportedFiles/GetCategoriesByProductsCountXML.xml", xmlString);
            }

        }

        //4.2. XML
        static void GetSoldProductXML()
        {
            using (var db = new ProductsShopContext())
            {
            var users = db.Users
                .Where(u => u.SelledProducts.Count() >= 1)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    products = u.SelledProducts.Select(p => new
                    {
                        name = p.Name,
                        price = p.Price
                    })
                }).OrderBy(u => u.lastName)
                .ThenBy(u => u.firstName)
                .ToArray();

            XDeclaration declaration = new XDeclaration("1.0", "utf-8", null);
            XDocument xDoc = new XDocument(declaration);

            var xmlDoc = new XDocument(new XElement("users")); // това е root-a

            foreach (var user in users)
            {
                var userNames = new XElement("user");

                if (user.firstName != null)
                {
                    userNames.Add(new XAttribute("first-name", user.firstName));
                }

                userNames.Add(new XAttribute("last-name", user.lastName),
                    new XElement("sold-products"));                                                             

                foreach (var p in user.products)
                {
                    userNames.Add(new XElement("product",
                            new XElement("name", p.name),
                            new XElement("price", p.price)));
                }
                xmlDoc.Root.Add(userNames);

            }
            string xmlString = xDoc.Declaration + Environment.NewLine + xmlDoc;
            //string xmlString = xmlDoc.ToString();
            File.WriteAllText("ExportedFiles/GetSoldProductXML.xml", xmlString);
            }
        }

        //4.1. XML
        static void GetProductsInRangeXML()
        {
            using (var db = new ProductsShopContext())
            {
                var products = db.Products
                    .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.BuyerId != null)
                    .Select(b => new
                    {
                        name = b.Name,
                        price = b.Price,
                        buyer = $"{b.Buyer.FirstName} {b.Buyer.LastName}"
                    }).OrderBy(p => p.price)
                    .ToArray();

                XDeclaration declaration = new XDeclaration("1.0", "utf-8", null);
                XDocument xDoc = new XDocument(declaration);
                
                var xmlDoc = new XDocument(new XElement("products")); // това е root-a

                foreach (var product in products)
                {
                    xmlDoc.Root.Add(new XElement("product",
                        new XAttribute("name", product.name),
                        new XAttribute("price", product.price),
                        new XAttribute("buyer", product.buyer)));
                }
                string xmlString = xDoc.Declaration + Environment.NewLine + xmlDoc;
                //string xmlString = xmlDoc.ToString();
                File.WriteAllText("ExportedFiles/GetProductsInRangeXML.xml", xmlString);

                //xmlDoc.Save("ExportedFiles/GetProductsInRangeXML.xml");  // var.2 for Save              
            }

        }

        //3.1. 3-rd
        static string ImportProductsFromXml()
        {
            var path = "Files/products.xml";
            var xmlString = File.ReadAllText(path);
            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var catProducts = new List<CategoryProduct>();

            using (var context = new ProductsShopContext())
            {
                var userIds = context.Users.Select(u => u.Id).ToArray();
                var categoryIds = context.Categories.Select(u => u.Id).ToArray();

                Random rnd = new Random();

                foreach (var e in elements)
            {
                    
                    string name = e.Element("name").Value;
                    decimal price = decimal.Parse(e.Element("price").Value);

                    int sellerIndex = rnd.Next(0, userIds.Length);
                    int sellerId = userIds[sellerIndex];
                                                      
                    var product = new Product()
                    {
                    Name = name,
                    Price = price,
                    SellerId = sellerId
                    };

                    int categoryIndex = rnd.Next(0, categoryIds.Length);
                    int categoryId = userIds[categoryIndex];

                    var catProduct = new CategoryProduct()
                    {
                        Product = product,
                        CategoryId = categoryId
                    };

                    catProducts.Add(catProduct);
            }
           
                context.AddRange(catProducts);
                context.SaveChanges();
            }
            return $"{catProducts.Count} categories were imported from file: {path}";
        }               

        //3.1. 2-nd
        static string ImportCategoriesFromXml()
        {
            var path = "Files/categories.xml";
            var xmlString = File.ReadAllText(path);
            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();


            var categories = new List<Category>();

            foreach (var e in elements)
            {
                string name = e.Element("name").Value;
                
                var category = new Category()
                {
                    Name = name,                    
                };

              categories.Add(category);
            }

            using (var context = new ProductsShopContext())
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
            return $"{categories.Count} categories were imported from file: {path}";
        }
        //3.1. 1-st
        static string ImportUsersFromXml()
        {
            var path = "Files/users.xml";
            var xmlString = File.ReadAllText(path);
            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();


            var users = new List<User>();

            foreach (var e in elements)
            {
                string firstName = e.Attribute("firstName")?.Value;
                string lastName = e.Attribute("lastName").Value;

                // За nullable стринг слагаме ?, но за nullable int трябва:
                int? age = null;
                if(e.Attribute("age") != null)
                {
                    age = int.Parse(e.Attribute("age").Value);
                }

                var user = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age
                };

                users.Add(user);
            }

            using (var context = new ProductsShopContext())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            return $"{users.Count} users were imported from file: {path}";
        }
     

        //2.4
        static void GetUsersAndProducts()
        {
            using (var db = new ProductsShopContext())
            {
                var users = db.Users
                    .Where(u => u.SelledProducts.Count() >= 1)
                    .OrderByDescending(sp => sp.SelledProducts.Count())
                    .ThenBy(u => u.LastName)
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        age = u.Age,
                        soldProducts = new
                        {
                            count = u.SelledProducts.Count(),
                            products = u.SelledProducts.Select(sp => new
                            {
                                name = sp.Name,
                                price = sp.Price
                            })
                        }
                       
                    }).ToArray();

                var usersToSerialize = new
                {
                    usersCount = users.Count(),
                    users = users
                };

                var jsonString = JsonConvert.SerializeObject(usersToSerialize, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });
                File.WriteAllText("ExportedFiles/2.4.UsersAndProducts.json", jsonString);
            }
        }
        
        //2.3.
        static void GetCategoriesByProductsCount()
        {
            using (var db = new ProductsShopContext())
            {
                var categories = db.Categories
                    .Select(c => new
                    {
                        category = c.Name,
                        productsCount = c.CategoryProducts.Count(),
                        averagePrice = c.CategoryProducts.Average(p => p.Product.Price),
                        totalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                    })
                    .OrderBy(c => c.category)
                    .ToArray();

                //var categories = db.Categories
                //        .Include(cp => cp.CategoryProducts)
                //        .ThenInclude(p => p.Product)
                //        .OrderBy(c => c.Name)
                //        .ToArray();
                //var categoryByProd = categories
                //     .Select(c => new
                //     {
                //         category = c.Name,
                //         productsCount = c.CategoryProducts.Count(),
                //         products = c.CategoryProducts.Average(p => p.Product.Price)
                //          totalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                //     });

                var jsonString = JsonConvert.SerializeObject(categories, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });
                
                File.WriteAllText("ExportedFiles/2.3.CategoriesByProductsCount.json", jsonString);                
            }
        }

        //2.2.
        static void GetSuccessfullySoldProducts()
        {
            using (var db = new ProductsShopContext())
            {
                var users = db.Users
                    .Where(u => u.SelledProducts.Count >= 1)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        soldProducts = u.SelledProducts.Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })
                    }).ToArray();

                var jsonString = JsonConvert.SerializeObject(users, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });

                Console.WriteLine();
                File.WriteAllText("ExportedFiles/2.2.SuccessfullySoldProducts.json", jsonString);


            }
        }
        
        //2.1.
        static void GetProductsInRange()
        {
            using (var db = new ProductsShopContext())
            {
                var products = db.Products
                    .Where(p => p.Price >= 500 && p.Price <= 1000)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = $"{p.Price:f2}",
                        seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                    })
                    .OrderBy(p => p.price)
                    .ToArray();

                var jsonProduct = JsonConvert.SerializeObject(products, Formatting.Indented);

                File.WriteAllText("ExportedFiles/2.1.PricessInRange.json", jsonProduct);
            }
        }

        //1.
        static void SetCategoryes()
        {
            using (var context = new ProductsShopContext())
            {
                // за всеки продукт искаме да сложим 3 случайни категории:
                var productIds = context.Products.AsNoTracking().Select(p => p.Id)
                    .ToArray();

                var categoryIds = context.Categories.AsNoTracking().Select(c => c.Id)
                    .ToArray();

                int categoryCount = categoryIds.Length;

                Random rnd = new Random();

                var categoryProducts = new List<CategoryProduct>();
                foreach (var p in productIds)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int index = rnd.Next(0, categoryCount);
                        while (categoryProducts.Any(cp => cp.ProductId == p
                        && cp.CategoryId == categoryIds[index]))
                        {
                            index = rnd.Next(0, categoryCount);
                        }

                        var catPr = new CategoryProduct()
                        {
                            ProductId = p,
                            CategoryId = categoryIds[index]
                        };

                        categoryProducts.Add(catPr);
                    }

                }
                context.CategoryProducts.AddRange(categoryProducts);
                context.SaveChanges();
            }

        }

        static string ImportProductsFromJson()
        {
            string path = "Files/products.json";

            //понеже в файла няма id-та за salerId, трябва сложим случайни от users:
            Random rnd = new Random();

            Product[] products = ImportJson<Product>(path);

            using (var context = new ProductsShopContext())
            {
                int[] userIds = context.Users.Select(u => u.Id).ToArray();


                foreach (var p in products)
                {
                    int index = rnd.Next(0, userIds.Length);
                    int sellerId = userIds[index];

                    int? buyerId = sellerId;        // !
                    while (buyerId == sellerId)
                    {
                        int buyerIndex = rnd.Next(0, userIds.Length);
                        buyerId = userIds[buyerIndex];
                    }

                    if (buyerId - sellerId < 5 && buyerId - sellerId > 0)
                    {
                        buyerId = null;
                    }

                    p.SellerId = sellerId;
                    p.BuyerId = buyerId;
                }

                context.AddRange(products);

                context.SaveChanges();
            }

            return $"{products.Length} products were imported from file: {path}";
        }

        static string InportCategoriesFromJson()
        {
            string path = "Files/categories.json";

            Category[] categories = ImportJson<Category>(path);

            using (var context = new ProductsShopContext())
            {
                context.AddRange(categories);

                context.SaveChanges();
            }

            return $"{categories.Length} categories were imported from file: {path}";
        }

        static string ImportUsersFromJson()
        {
            string path = "Files/users.json";

            User[] users = ImportJson<User>(path);

            using (var context = new ProductsShopContext())
            {
                context.AddRange(users);

                context.SaveChanges();
            }

            return $"{users.Length} users were imported from file: {path}";
        }

        static T[] ImportJson<T>(string path) 
        // Generic метод за импорт на jeson от този път и обръщане в обекти
        {
            string jsonStrintg = File.ReadAllText(path);

            T[] objects = JsonConvert.DeserializeObject<T[]>(jsonStrintg);

            return objects;
        }
    }
}
