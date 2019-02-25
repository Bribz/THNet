using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THServerEngine
{
    public enum LogType
    {
        SYSTEM,
        SERVER,
        ERROR,
        WARNING,
        DEFAULT,
        DEBUG,
        VERBOSE
    }

    public class Log
    {
        public static int MAX_LOG_TYPE = 2;

        public static void SetLogMaximum(LogType maximum)
        {
            MAX_LOG_TYPE = (int)maximum;
        }

        public static void Write(string msg, LogType type)
        {
            if ((int)type > MAX_LOG_TYPE)
            {
                return;
            }

            Console.Write($"\n[{Enum.GetName(typeof(LogType), type)}] : {msg}");
        }
    }
}
