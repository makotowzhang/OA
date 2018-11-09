using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DateTimeExtensions
    {
        public static string ToCommonDateTime(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToCommonDataTime(this DateTime? value)
        {
            if (value.HasValue)
            {
                return "";
            }
            else
            {

                return value.ToCommonTime();
            }
        }
        public static string ToCommonDate(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }
        public static string ToCommonDate(this DateTime? value)
        {
            if (value.HasValue)
            {
                return "";
            }
            else
            {

                return value.ToCommonDate();
            }
        }
        public static string ToCommonTime(this DateTime value)
        {
            return value.ToString("HH:mm:ss");
        }
        public static string ToCommonTime(this DateTime? value)
        {
            if (value.HasValue)
            {
                return "";
            }
            else
            {

                return value.ToCommonTime();
            }
        }
    }
}
