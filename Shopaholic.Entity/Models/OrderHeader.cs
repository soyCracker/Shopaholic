using System;
using System.Collections.Generic;

#nullable disable

namespace Shopaholic.Entity.Models
{
    public partial class OrderHeader
    {
        public OrderHeader()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public string Shipnumber { get; set; }
        public int? FailCode { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
