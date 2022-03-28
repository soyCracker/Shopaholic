using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class EcPayReq
    {
        public string MerchantID { get; set; }
        public string MerchantTradeNo { get; set; }
        public string MerchantTradeDate { get; set; }
        public string PaymentType { get; set; } = "aio";
        public int TotalAmount { get; set; }
        public string TradeDesc { get; set; } = "Shopaholic 商城購物";
        public string ItemName { get; set; }
        public string ReturnURL { get; set; }
        public string ChoosePayment { get; set; } = "Credit";
        public string CheckMacValue { get; set; }
        public string ClientBackURL { get; set; }
        
    }
}
