using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using THClientEngine;

namespace THClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            Log.SetLogPlatform(LoggingPlatform.Console);
            Log.SetLogMaximum(LogType.VERBOSE);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7735);
            THNet_Client client = new THNet_Client(ipep, true);

            Console.ReadKey();
        }
    }
}
