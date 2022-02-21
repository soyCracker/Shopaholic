using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Common.Constants
{
    public class OrderStateCode
    {
        public static readonly int CREATE = 0;
        public static readonly int PAID = 1;
        public static readonly int PICKUP = 2;
        public static readonly int SENT = 3;
        public static readonly int ARRIVED = 4;
    }
}
