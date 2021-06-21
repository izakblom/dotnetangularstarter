using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utilities
{
    public static class TimeHelper
    {
        public static DateTime ZATimeNow()
        {

            TimeZoneInfo zaTimeZone = TimeHelper.GetTimeZone("South Africa Standard Time")
                ?? TimeZoneInfo.FindSystemTimeZoneById("Africa/Johannesburg")
                ?? TimeZoneInfo.Local;


            var now = TimeHelper.ToSpecificTimeZone(DateTime.UtcNow, zaTimeZone);

            return now;

        }

        public static string ConvertSecondsToDaysHoursMinutes(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            if (time.Days > 0)
                return time.ToString(@"d'd, 'hh\:mm\:ss");

            return time.ToString(@"hh\:mm\:ss");

        }

        public static DateTime ToSpecificTimeZone(this DateTime source, TimeZoneInfo timeZone)
        {
            var offset = timeZone.GetUtcOffset(source);
            var newDt = source + offset;
            return newDt;
        }

        public static TimeZoneInfo GetTimeZone(string timezoneID)
        {

            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timezoneID);

                return tz;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Could not get timezone: {0}: {1}", timezoneID, ex.ToString());
                return null;
            }

        }

        public static TimeZoneInfo GetSouthAfricanTimeZone()
        {
            var zaTimeZone = GetTimeZone("South Africa Standard Time")
                ?? GetTimeZone("Africa/Johannesburg")
                ?? TimeZoneInfo.Local;

            return zaTimeZone;

        }

        public static double DaysBetween(DateTime closestFromNow, DateTime farthestFromNow)
        {
            return (closestFromNow - farthestFromNow).TotalDays;
        }

        public static Dictionary<string, List<DateTime>> GetMonthSpans(int months)
        {
            var res = new Dictionary<string, List<DateTime>>();

            var now = ZATimeNow();

            for (var x = 1; x < (months + 1); x++)
            {
                var from = new DateTime(now.Date.Year, now.Date.Month, 1);
                var to = new DateTime(now.Date.Year, now.Date.Month, DateTime.DaysInMonth(now.Date.Year, now.Date.Month));

                res.Add(from.Date.ToString("MMM \\'yy"), new List<DateTime>() { from, to });

                now = now.AddMonths(-1);
            }

            return res;
        }
    }

    public class DTODateRange
    {
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
    }
}
