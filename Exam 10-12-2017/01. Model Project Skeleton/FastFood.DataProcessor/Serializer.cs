using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace FastFood.DataProcessor
{
    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
			
            var orderTypeEnum = Enum.Parse<OrderType>(orderType);

            // Variant 1:
            var orders = context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .Where(o => o.Employee.Name == employeeName)
                .Select(o => new
                {
                    o.Customer,
                    Items = o.OrderItems
                        .Select(oi => new
                        {
                            Name = oi.Item.Name,
                            Price = oi.Item.Price,
                            Quantity = oi.Quantity
                        }),
                    TotalPrice = o.OrderItems
                        .Sum(oi => oi.Quantity * oi.Item.Price)
                })
                .OrderByDescending(o => o.TotalPrice)
                .ThenByDescending(o => o.Items.Count())
                .ToList();

            // Variant 2:			
            //var orders = context.Orders.Where(o => o.Employee.Name == employeeName && o.Type == type)
            //    .Select(o => new 
            //    {
            //        Customer = o.Customer,
            //        Items = o.OrderItems.Select(i => 
            //        {
            //            Name = i.Item.Name,
            //            Price = i.Item.Price,
            //            Quantity = i.Quantity
            //        }),
            //        TotalPrice = o.OrderItems.Sum(i => i.Quantity * i.Item.Price)
            //    })//.ToArray()
            //    .OrderByDescending(o => o.TotalPrice)
            //    .OrderByDescending(o => o.Items.Count())
            //    .ToList();    

            var result = new
            {
                Name = employeeName,
                Orders = orders,
                TotalMade = orders.Sum(o => o.TotalPrice)
            };

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);
            return json;
			
            // or
            //var jsonString = JsonConvert.SerializeObject(employee, Formatting.Indented,
            //          new JsonSerializerSettings()
            //          {
            //              DefaultValueHandling = DefaultValueHandling.Ignore
            //          });
            //return jsonString;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            // Variant 1:
            // var requestedCategories = categoriesString.Split(',').ToArray();

            // var selectedCategories = context.Categories
            //     .Include(c => c.Items)
            //     .ThenInclude(i => i.OrderItems) 
            //     .Where(c => requestedCategories.Contains(c.Name))
            //     .ToArray();

            // var categories = selectedCategories.Select(c => new
            // {
            //     Name = c.Name,
            //     MostPopularItem =c.Items.Select(i=>new
            //     {
            //         Name = i.Name,
            //         TotalMade = i.Price * i.OrderItems.Sum(oi => oi.Quantity),
            //         TimesSold = i.OrderItems.Sum(io=>io.Quantity)
            //     })
            //     .OrderByDescending(i=>i.TotalMade)
            //     .OrderByDescending(i => i.TimesSold)
            //     .Take(1)
            // }).ToArray();

            // var xmlDoc=new XDocument(new XElement("Categories"));
            // ////or this:
            // //var xmlDoc = new XDocument();
            // //xmlDoc.Add(new XElement("Categories"));

            // foreach (var c in categories)
            // {
            //     var category = new XElement("Category");
            //     category.Add(new XElement("Name", c.Name));

            //     var mostPopularItems = (new XElement("MostPopularItem"));

            //     foreach (var item in c.MostPopularItem)
            //     {
            //         mostPopularItems.Add(new XElement("Name", item.Name));
            //         mostPopularItems.Add(new XElement("TotalMade", item.TotalMade));
            //         mostPopularItems.Add(new XElement("TimesSold", item.TimesSold));
            //     }

            //     category.Add(mostPopularItems);
            //     xmlDoc.Root.Add(category);
            // }

            // string xmlString = xmlDoc.ToString(); 
            //return xmlString;

            // Variant 2 - with Dto and autoserelization:
            var requestedCategories = categoriesString.Split(',').ToArray();

            var selectedCategories = context.Categories
                .Include(c => c.Items)
                .ThenInclude(i => i.OrderItems)
                .Where(c => requestedCategories.Contains(c.Name))
                .ToArray();

            var categories = selectedCategories.Select(c => new CategoryDto
            {
                Name = c.Name,
                MostPopularItem = c.Items.Select(i => new MostPopularItemDto
                {
                    Name = i.Name,
                    TotalMade = i.Price * i.OrderItems.Sum(oi => oi.Quantity),
                    TimesSold = i.OrderItems.Sum(io => io.Quantity)
                })
                .ToArray()
                .OrderByDescending(i => i.TotalMade)
                .OrderByDescending(i => i.TimesSold)
                .FirstOrDefault()
            }).ToArray();

           
            var sb = new StringBuilder();

                var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));
                serializer.Serialize(new StringWriter(sb), categories, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            
                var result = sb.ToString();
                return result;            
        }
    }
}