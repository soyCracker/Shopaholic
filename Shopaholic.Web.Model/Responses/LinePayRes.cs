using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Responses
{
    public class LinePayRes
    {
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
        public Info info { get; set; }
    }
    public class Info
    {
        public Paymenturl paymentUrl { get; set; }
        public long transactionId { get; set; }
        public string paymentAccessToken { get; set; }
    }
    public class Paymenturl
    {
        public string web { get; set; }
        public string app { get; set; }
    }
}
