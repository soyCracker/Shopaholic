using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Utilities
{
    public class TimeUtil
    {
        public static readonly string yyyyMMddddFormat = "yyyy/MM/dd";

        public static DateTime GetLocalDateTime()
        {
            return DateTime.Now;
        }

        public static string DateTimeToYYYYMMdd(DateTime targetTime, string timeFormat)
        {
            return targetTime.ToString(timeFormat);
        }
    }
}
