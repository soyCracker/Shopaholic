using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class PurchaseReq
    {
        public PurchaseProductListModel ProductList { get; set; }
        public int OrderTypeCode { get; set; }
        public string UserId { get; set; }
        public string ReceiveMan { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
