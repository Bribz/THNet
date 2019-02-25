using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THServerEngine.Util
{
    public static class Time
    {
        public static string GetRunTimeOffset(DateTime startTime, DateTime endTime)
        {
            string retVal = "";
            var timeSpan = endTime - startTime;

            if(timeSpan.Days > 0)
            {
                retVal += $"{timeSpan.Days} Days, ";
            }

            if (timeSpan.Hours > 0)
            {
                retVal += $"{timeSpan.Hours} Hours, ";
            }

            if (timeSpan.Minutes > 0)
            {
                retVal += $"{timeSpan.Minutes} Minutes, ";
            }

            if (timeSpan.Seconds > 0)
            {
                retVal += $"{timeSpan.Seconds} Seconds, ";
            }

            if (timeSpan.Minutes <= 0)
            {
                retVal += $"{timeSpan.Milliseconds} Milliseconds";
            }

            return retVal;
        }
    }
}
