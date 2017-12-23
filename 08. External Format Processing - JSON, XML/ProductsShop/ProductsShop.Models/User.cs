using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ProductsShop.Models
{
    public class User
    {
        public int Id { get; set; }
        public int? Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Product> BuyedProducts { get; set; } = new List<Product>();
        public ICollection<Product> SelledProducts { get; set; } = new List<Product>();
    }
}
