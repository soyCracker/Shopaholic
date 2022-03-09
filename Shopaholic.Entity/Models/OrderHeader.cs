using System;
using System.Collections.Generic;

namespace Shopaholic.Entity.Models
{
    public partial class OrderHeader
    {
        public OrderHeader()
        {
            OrderDetails = new HashSet<OrderDetail>();
            OrderLogs = new HashSet<OrderLog>();
            Shipments = new HashSet<Shipment>();
        }

        public int Id { get; set; }
        public string OrderId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int StateCode { get; set; }
        public string? Remark { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public int? FailCode { get; set; }
        public int OrderTypeCode { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsSent { get; set; }
        public bool? IsArrived { get; set; }
        public bool? IsCancel { get; set; }
        public bool? IsReturn { get; set; }
        public bool? IsFinish { get; set; }
        public bool? IsDelete { get; set; }

        public virtual LinePayOrder LinePayOrder { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
