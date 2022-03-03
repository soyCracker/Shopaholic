using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class AddToCartReq
    {
        public string AccountId { get; set; }
        public string Email { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
