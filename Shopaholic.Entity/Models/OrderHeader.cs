using System;
using System.Collections.Generic;

#nullable disable

namespace Shopaholic.Entity.Models
{
    public partial class OrderHeader
    {
        public OrderHeader()
        {
            OrderLogs = new HashSet<OrderLog>();
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public int StateCode { get; set; }
        public string Remark { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public string ShipNumber { get; set; }
        public int? FailCode { get; set; }
        public int OrderTypeCode { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsSent { get; set; }
        public bool? IsArrived { get; set; }
        public bool? IsCancel { get; set; }
        public bool? IsReturn { get; set; }
        public bool? IsFinish { get; set; }
        public bool? IsDelete { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
    }
}
