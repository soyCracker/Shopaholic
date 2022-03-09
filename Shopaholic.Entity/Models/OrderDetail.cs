using System;
using System.Collections.Generic;

namespace Shopaholic.Entity.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public string OrderId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int CurrentPrice { get; set; }
        public int RealQuantity { get; set; }
        public bool IsBroken { get; set; }
        public int BrokenQuantity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual OrderHeader Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
