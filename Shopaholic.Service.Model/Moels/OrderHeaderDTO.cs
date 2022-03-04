using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Model.Moels
{
    public class OrderHeaderDTO
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public bool? IsFinish { get; set; }
        public bool? IsSent { get; set; }    
        public int Status { get; set; }       
        public string FormatCreateTime { get; set; }
        public string ShipNumber { get; set; }
        public int? FailCode { get; set; }
        public string StatusMsg { get; set; }
    }
}
