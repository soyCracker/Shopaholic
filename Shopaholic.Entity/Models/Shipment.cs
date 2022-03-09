using System;
using System.Collections.Generic;

namespace Shopaholic.Entity.Models
{
    public partial class Shipment
    {
        public int Id { get; set; }
        public string OrderId { get; set; } = null!;
        public string? ShipNumber { get; set; }
        public string ReceiveMan { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Email { get; set; }

        public virtual OrderHeader Order { get; set; } = null!;
    }
}
