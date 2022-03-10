using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Responses
{
    public class PurchasePayRes
    {
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public string CallbackUrl { get; set; }
    }
}
