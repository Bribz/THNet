using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace THClientEngine
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

    public enum LoggingPlatform
    {
        Console,
        Unity
    }

    public class Log
    {
        public static LoggingPlatform platform = LoggingPlatform.Unity;

        public static int MAX_LOG_TYPE = 2; // 2 = Error

        public static void SetLogMaximum(LogType maximum)
        {
            MAX_LOG_TYPE = (int)maximum;
        }

        public static void SetLogPlatform(LoggingPlatform platformType)
        {
            platform = platformType;
        }

        public static void Write(string msg, LogType type)
        {
            if ((int)type > MAX_LOG_TYPE)
            {
                return;
            }

            if (platform == LoggingPlatform.Console)
            {
                Console.Write($"\n[{Enum.GetName(typeof(LogType), type)}] : {msg}");
            }
            else
            {
                switch(type)
                {
                    case LogType.VERBOSE:
                    case LogType.DEBUG:
                    case LogType.DEFAULT:
                    case LogType.SERVER:
                    case LogType.SYSTEM:
                    default:
                        Debug.Log($"\n[{Enum.GetName(typeof(LogType), type)}] : {msg}");
                        break;

                    case LogType.WARNING:
                        Debug.LogWarning($"\n[{Enum.GetName(typeof(LogType), type)}] : {msg}");
                        break;

                    case LogType.ERROR:
                        Debug.LogError($"\n[{Enum.GetName(typeof(LogType), type)}] : {msg}");
                        break;
                }
            }
        }
    }
}
