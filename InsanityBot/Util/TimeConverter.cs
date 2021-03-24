using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace InsanityBot.Util
{
    public static class TimeConverter
    {
        public static TimeSpan GetTimeSpan(Int16 Time, Char TimeID)
        {
            return TimeID switch
            {
                's' => TimeSpan.FromSeconds(Time),
                'm' => TimeSpan.FromMinutes(Time),
                'h' => TimeSpan.FromHours(Time),
                'd' => TimeSpan.FromDays(Time),
                _ => TimeSpan.FromMilliseconds(-1)
            };
        }
    }
}
