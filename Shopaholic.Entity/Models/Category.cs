using System;
using System.Collections.Generic;

namespace Shopaholic.Entity.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
