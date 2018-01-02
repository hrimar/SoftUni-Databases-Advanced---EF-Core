using System;
using FastFood.Data;
using System.Text;
using FastFood.Models;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using FastFood.DataProcessor.Dto.Import;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using FastFood.Models.Enums;

namespace FastFood.DataProcessor
{
	public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
            var sb = new StringBuilder();
           EmployeeDto[] deserializedEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            var validdEmployees = new List<Employee>();


            foreach (var employeeDto in deserializedEmployees)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var position = context.Positions.SingleOrDefault(p => p.Name == employeeDto.Position);

                if(position==null)
                {
                    Position newPosition = new Position()
                    {
                        Name = employeeDto.Position
                    };
                    context.Positions.Add(newPosition);
                    context.SaveChanges();

                    var employee = new Employee()
                    {
                        Name = employeeDto.Name,
                        Age = employeeDto.Age,
                        Position = newPosition
                    };
                    validdEmployees.Add(employee);
                    sb.AppendLine(string.Format(SuccessMessage, employeeDto.Name));
                }
                else
                {
                    var employee = new Employee()
                    {
                        Name = employeeDto.Name,
                        Age = employeeDto.Age,
                        Position = position
                    };
                    validdEmployees.Add(employee);
                    sb.AppendLine(string.Format(SuccessMessage, employeeDto.Name));
                }               
            }
            context.Employees.AddRange(validdEmployees);
            context.SaveChanges(); 

            return sb.ToString();
		}

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
         ItemDto[] deserializedItems = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            var validItems = new List<Item>();
            
            foreach (var itemDto in deserializedItems)
            {
                if (!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                
                var isItemExists = validItems.Any(i => i.Name == itemDto.Name);
                if (isItemExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                var category = context.Categories.SingleOrDefault(c => c.Name == itemDto.Category);
                if(category==null)
                {
                    var newCategory = new Category()
                    {
                        Name = itemDto.Category
                    };
                    context.Categories.Add(newCategory);
                    context.SaveChanges();

                    var item = new Item()
                    {
                        Name = itemDto.Name,
                        Price = itemDto.Price,
                        Category = newCategory
                    };
                    validItems.Add(item);
                    sb.AppendLine(string.Format(SuccessMessage, itemDto.Name));
                }
                else
                {
                    var item = new Item()
                    {
                        Name = itemDto.Name,
                        Price = itemDto.Price,
                        Category = category
                    };
                    validItems.Add(item);
                    sb.AppendLine(string.Format(SuccessMessage, itemDto.Name));
                }

            }
            context.Items.AddRange(validItems);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            var deserializedOrders = (OrderDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();

            var validOrders = new List<Order>();

            foreach (var orderDto in deserializedOrders)
            {
                if (!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime date =
                    DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                              
              var typeE = Enum.TryParse<OrderType>(orderDto.Type, out var type) ? type : OrderType.ForHere;
              
             
               

                var isEmployeeExists = context.Employees
                    .SingleOrDefault(e => e.Name == orderDto.Employee);

                if(isEmployeeExists==null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var items = orderDto.Items.ToArray();           // !!!!!!!!!!!!
                // за проверка дали всички нещя от кол-я са валидни:
                var isValidItems = items.All(i => IsValid(items));         // !!!!!!!!!!!!
                if(!isValidItems)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                // проверка дали всички items от Dto-то същест-т в базата:
                var isAllItemsExist = orderDto.Items.All(i => context.Items.Any(ii => ii.Name == i.Name));
                if(!isAllItemsExist)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                //var isEmployeeUnique = validOrders.Any(e => e.Employee.Name == orderDto.Employee);
                //if (isEmployeeUnique)
                //{
                //    sb.AppendLine(FailureMessage);
                //    continue;
                //}

                var orderItems = new List<OrderItem>();
                foreach (var itemsDto in orderDto.Items)
                {
                    var item = context.Items.SingleOrDefault(i => i.Name == itemsDto.Name);

                    var orderItem = new OrderItem()
                    {
                        Item = item,
                        Quantity = itemsDto.Quantity
                    };
                    orderItems.Add(orderItem);
                }
                

                var order = new Order()
                {
                    Customer = orderDto.Customer,
                    DateTime = date,
                    Employee = isEmployeeExists,
                    Type = typeE,
                    OrderItems = orderItems
                };
                validOrders.Add(order);
                sb.AppendLine($"Order for {orderDto.Customer} on {orderDto.DateTime} added");
            }
            context.Orders.AddRange(validOrders);
            context.SaveChanges();

            return sb.ToString();
            }

            private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}