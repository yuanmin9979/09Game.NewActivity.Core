using System;
using System.Collections.Generic;
using System.Text;

namespace ItemSystem.Api.Utility
{
    public static class DatetimeUtility
    {
        private static DateTime _base = new DateTime(1970, 1, 1);
        public static long UniversalTimestamp(this DateTime dt)
        {
            return (long)dt.ToUniversalTime().Subtract(_base).TotalSeconds;
        }


    }
}
