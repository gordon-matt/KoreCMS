﻿using System;

namespace Kore
{
    public static class DateTimeExtensions
    {
        public static bool IsNullOrDefault(this DateTime? source)
        {
            return source == null || source == default(DateTime);
        }

        public static string ToISO8601DateString(this DateTime source)
        {
            return source.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        }

        public static DateTime ParseUnixTimestamp(int unixTimestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(unixTimestamp)
                .ToLocalTime();
        }

        public static string ToRelative(this DateTime source, bool convertToUserTime = false, string defaultFormat = null)
        {
            string result;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - source.Ticks);
            var delta = ts.TotalSeconds;

            if (delta > 0)
            {
                if (delta < 60) // 60 (seconds)
                {
                    result = ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
                }
                else if (delta < 120) //2 (minutes) * 60 (seconds)
                {
                    result = "a minute ago";
                }
                else if (delta < 2700) // 45 (minutes) * 60 (seconds)
                {
                    result = ts.Minutes + " minutes ago";
                }
                else if (delta < 5400) // 90 (minutes) * 60 (seconds)
                {
                    result = "an hour ago";
                }
                else if (delta < 86400) // 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int hours = ts.Hours;
                    if (hours == 1)
                        hours = 2;
                    result = hours + " hours ago";
                }
                else if (delta < 172800) // 48 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = "yesterday";
                }
                else if (delta < 2592000) // 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = ts.Days + " days ago";
                }
                else if (delta < 31104000) // 12 (months) * 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    result = months <= 1 ? "one month ago" : months + " months ago";
                }
                else
                {
                    int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    result = years <= 1 ? "one year ago" : years + " years ago";
                }
            }
            else
            {
                DateTime tmp1 = source;
                if (convertToUserTime)
                {
                    //TODO
                    //tmp1 = EngineContext.Current.Resolve<IDateTimeHelper>().ConvertToUserTime(tmp1, DateTimeKind.Utc);
                }

                //default formatting
                if (!string.IsNullOrEmpty(defaultFormat))
                {
                    result = tmp1.ToString(defaultFormat);
                }
                else
                {
                    result = tmp1.ToString();
                }
            }
            return result;
        }

        public static double ToUnixTimestamp(this DateTime source)
        {
            return (source - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()).TotalSeconds * 1000;
        }

        /// <summary>
        /// Gets the beginning of the current month - including time.
        ///
        /// </summary>
        /// <param name="source">The date time object being extended..</param>
        /// <returns>
        /// Beginning of the current month.
        /// </returns>
        public static DateTime BeginningThisMonth(this DateTime source)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, source.Kind);
        }

        /// <summary>
        /// Gets the beginning of the current week - including time.
        ///
        /// </summary>
        /// <param name="source">The date time object being extended..</param>
        /// <returns>
        /// Beginning of the current week.
        /// </returns>
        public static DateTime BeginningThisWeek(this DateTime source)
        {
            DateTime dateTime = source;
            while (dateTime.DayOfWeek != DayOfWeek.Monday)
            {
                dateTime = dateTime.AddDays(-1.0);
            }
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, source.Kind);
        }

        public static DateTime StartOfWeek(this DateTime source, DayOfWeek startOfWeek)
        {
            var diff = source.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return source.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime source, DayOfWeek endOfWeek)
        {
            var diff = endOfWeek - source.DayOfWeek;
            return source.AddDays(diff).Date;
        }

        public static DateTime StartOfMonth(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, 1, 0, 0, 0, source.Kind);
        }

        public static DateTime StartOfYear(this DateTime source)
        {
            return new DateTime(source.Year, 1, 1, 0, 0, 0, source.Kind);
        }
    }
}