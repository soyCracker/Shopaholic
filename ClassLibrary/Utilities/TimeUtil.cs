using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Utilities
{
    public class TimeUtil
    {
        public static readonly string yyyyMMddFormat = "yyyy/MM/dd";
        public static readonly string yyyyMMddFormat_02 = "yyyyMMdd";

        public static DateTime GetLocalDateTime()
        {
            return DateTime.Now;
        }

        public static string DateTimeToFormatStr(DateTime targetTime, string timeFormat)
        {
            return targetTime.ToString(timeFormat);
        }

        public static DateTime GetLocalTodayDate()
        {
            return DateTime.Today;
        }
    }
}
