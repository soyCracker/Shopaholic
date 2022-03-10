using System;
using System.Collections.Generic;

namespace Shopaholic.Entity.Models
{
    public partial class LinePayOrder
    {
        public int Id { get; set; }
        public string OrderId { get; set; } = null!;
        public string LinePayOrderId { get; set; } = null!;
        public string? TransactionId { get; set; }

        public virtual OrderHeader Order { get; set; } = null!;
    }
}
