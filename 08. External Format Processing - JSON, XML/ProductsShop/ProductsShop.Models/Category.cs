using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        //[MinLength(3), MaxLength(15)] - taka ne stava
        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; } =
            new List<CategoryProduct>();
    }
}
