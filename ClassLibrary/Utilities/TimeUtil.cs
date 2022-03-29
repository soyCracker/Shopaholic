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

        public static DateTimeOffset GetUtcDateTime()
        {
            return DateTimeOffset.UtcNow;
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

        public static DateTime CovertToTaipeiDatetime(DateTime origin)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            return TimeZoneInfo.ConvertTime(origin, zone);           
        }

        public static DateTime CovertTaipeiToUtcDatetime(DateTime origin)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            return TimeZoneInfo.ConvertTimeToUtc(origin, zone);
        }
    }
}
