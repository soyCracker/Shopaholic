using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Common.Constants
{
    public class OrderFailCode
    {
        public static readonly int COMMON = 0;
        public static readonly int RETURN = 1;
        public static readonly int OVERDUE = 2;
        public static readonly int PICKUP_FAIL = 3;
        public static readonly int SHIP_FAIL = 4;
        public static readonly int UN_ARRIVE_CANCEL = 5;
    }
}
