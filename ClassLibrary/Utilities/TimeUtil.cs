using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Util.Utilities
{
    public class TimeUtil
    {
        public static readonly string yyyyMMdd_01 = "yyyy\\/MM\\/dd";
        public static readonly string yyyyMMdd_02 = "yyyyMMdd";
        public static readonly string yyyyMMdd_03 = "yyyy-MM-dd";
        public static readonly string yyyyMMddhhmmssFormat = "yyyy-MM-dd HH:mm:ss";
        public static readonly string yyyyMMddhhmmssFormat_02 = "yyyy\\/MM\\/dd HH:mm:ss";

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

        public static DateTime StrToLocalDateTime(string timeStr)
        {
            try
            {
                return DateTime.Parse(timeStr);
            }
            catch
            {
                // error format
            }
            return DateTime.MinValue;
        }


    }
}
