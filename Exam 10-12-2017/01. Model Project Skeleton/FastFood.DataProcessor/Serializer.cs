using System;
using System.IO;
using FastFood.Data;
using FastFood.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace FastFood.DataProcessor
{
	public class Serializer
	{
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var orderTypeEnum = Enum.Parse<OrderType>(orderType);

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

            var result = new
            {
                Name = employeeName,
                Orders = orders,
                TotalMade = orders.Sum(o => o.TotalPrice)
            };

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);
            return json;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
		{
            var requestedCategories = categoriesString.Split(',').ToArray();

            var selectedCategories = context.Categories
                .Include(c => c.Items)
                .ThenInclude(i => i.OrderItems) 
                .Where(c => requestedCategories.Contains(c.Name))
                .ToArray();

            var categories = selectedCategories.Select(c => new
            {
                Name = c.Name,
                MostPopularItem =c.Items.Select(i=>new
                {
                    Name = i.Name,
                    TotalMade = i.Price * i.OrderItems.Sum(oi => oi.Quantity),
                    TimesSold = i.OrderItems.Sum(io=>io.Quantity)
                })
                .OrderByDescending(i=>i.TotalMade)
                .OrderByDescending(i => i.TimesSold)
                .Take(1)
            }).ToArray();

            var xmlDoc=new XDocument(new XElement("Categories"));
            ////or this:
            //var xmlDoc = new XDocument();
            //xmlDoc.Add(new XElement("Categories"));

            foreach (var c in categories)
            {
                var category = new XElement("Category");
                category.Add(new XElement("Name", c.Name));

                var mostPopularItems = (new XElement("MostPopularItem"));

                foreach (var item in c.MostPopularItem)
                {
                    mostPopularItems.Add(new XElement("Name", item.Name));
                    mostPopularItems.Add(new XElement("TotalMade", item.TotalMade));
                    mostPopularItems.Add(new XElement("TimesSold", item.TimesSold));
                }

                category.Add(mostPopularItems);
                xmlDoc.Root.Add(category);
            }

            string xmlString = xmlDoc.ToString(); 
           return xmlString;
		}
	}
}