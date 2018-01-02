using System;
using FastFood.Data;
using System.Linq;

namespace FastFood.DataProcessor
{
    public static class Bonus
    {
	    public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
	    {
            var exactItem = context.Items.FirstOrDefault(i => i.Name == itemName);

            if (exactItem == null)
            {
                return $"Item {itemName} not found!";
            }

            decimal oldPrice = exactItem.Price;

            exactItem.Price = newPrice;
            context.SaveChanges();

            string result = $"{itemName} Price updated from ${oldPrice:F2} to ${newPrice:F2}";
            return result;
	    }
    }
}
