using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class PurchaseConfirmReq
    {
        public string SelfOrderId { get; set; }
        public string OtherSysOrderId { get; set; }
        public string TransactionId { get; set; }
    }
}
