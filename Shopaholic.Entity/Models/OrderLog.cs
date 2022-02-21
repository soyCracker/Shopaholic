using System;
using System.Collections.Generic;

#nullable disable

namespace Shopaholic.Entity.Models
{
    public partial class OrderLog
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public int StateCode { get; set; }
        public string Remark { get; set; }
        public int? FailCode { get; set; }
        public int OrderTypeCode { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual OrderHeader Order { get; set; }
    }
}
