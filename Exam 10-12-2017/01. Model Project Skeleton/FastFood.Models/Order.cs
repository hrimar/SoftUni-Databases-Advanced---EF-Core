﻿using FastFood.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FastFood.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public OrderType Type { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        public Employee Employee { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
