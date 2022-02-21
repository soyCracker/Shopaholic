using System;
using System.Collections.Generic;

#nullable disable

namespace Shopaholic.Entity.Models
{
    public partial class Shipment
    {
        public Shipment()
        {
            OrderHeaders = new HashSet<OrderHeader>();
        }

        public int Id { get; set; }
        public string ShipNumber { get; set; }
        public string ReceiveMan { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
