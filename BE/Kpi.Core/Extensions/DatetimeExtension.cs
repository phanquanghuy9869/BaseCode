using System;

namespace Kpi.Core.Extensions
{
    public static class DatetimeExtension
    {
        public static DateTime StartOfDay(this DateTime d)
        {
            return d.Date;
        }

        public static DateTime EndOfDay(this DateTime d)
        {
            return StartOfDay(d).AddDays(1).AddTicks(-1);
        }

        public static int GetYearMonth(this DateTime d)
        {
            var str = d.ToString("yyyyMM");
            return Convert.ToInt32(str);
        }
    }
}
