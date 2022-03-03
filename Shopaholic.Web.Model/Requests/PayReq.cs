using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class PayReq
    {
        public int Price { get; set; }
        public string OrderId { get; set; }
    }
}
