using System;
using System.Collections.Generic;

namespace Shopaholic.Entity.Models
{
    public partial class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string AccountId { get; set; } = null!;
    }
}
