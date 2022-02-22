using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Util.Utilities
{
    public class StringUtil
    {
        public static string AddZeroPadLeft(string str, int length)
        {
            return str.PadLeft(length, '0');
        }
    }
}
