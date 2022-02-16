using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Model.Moels
{
    public class OrderHeaderDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }       
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public string ShipNumber { get; set; }
        public int? FailCode { get; set; }
    }
}
