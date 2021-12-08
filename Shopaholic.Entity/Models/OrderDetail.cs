﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Shopaholic.Entity.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int RealQuantity { get; set; }
        public bool IsBroken { get; set; }
        public int BrokenQuantity { get; set; }

        public virtual OrderHeader Order { get; set; }
        public virtual Product Product { get; set; }
    }
}