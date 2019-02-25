using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THServerEngine;


namespace THServer
{
    public class CLI_INPUTS
    {
        public const char IS_PUBLIC = 'p';

        public const char DEBUG_VERBOSE = 'v';
        public const char DEBUG_ERROR = 'e';

        public const string MAX_PLAYERS = "max";
    }

    public class Program
    {
        public const string VERSION = "1.0.0";

        private static int maxPlayers = 64;
        private static bool Debug_ShowAll = false;
        private static bool Debug_ShowErrors = false;
        private static bool isLive = false;

        public static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string cli_input = args[i].Replace("-", "");

                AssignCLIInput(cli_input, args[i + 1]);
            }

            PrintServerLog();

            Log.SetLogMaximum(LogType.VERBOSE);
            THNet_Server _server = new THNet_Server(4, false, true);

            Console.ReadKey();
        }

        public static void AssignCLIInput(string variable, string next_val)
        {
            if (variable.Equals(CLI_INPUTS.IS_PUBLIC))
            {
                Program.isLive = true;
            }
            else if (variable.Equals(CLI_INPUTS.DEBUG_ERROR))
            {
                Program.Debug_ShowErrors = true;
            }
            else if (variable.Equals(CLI_INPUTS.DEBUG_VERBOSE))
            {
                Program.Debug_ShowAll = true;
            }
            else if (variable.Equals(CLI_INPUTS.MAX_PLAYERS))
            {
                try
                {
                    Int32.TryParse(next_val, out Program.maxPlayers);
                }
                catch (Exception E)
                {
                    Log.Write("Failed to parse maximum player count. Invalid input!", LogType.SYSTEM);
                }
            }
        }

        public static void PrintServerLog()
        {
            Console.WriteLine("\nStarting THNet Server " + VERSION + "\n");
            Console.WriteLine(" Initialized with settings:");
            if (Debug_ShowAll)
            {
                Console.WriteLine(" - Debugging set to VERBOSE");
            }
            else if (Debug_ShowErrors)
            {
                Console.WriteLine(" - Debugging set to ERROR_ONLY");
            }
            else
            {
                Console.WriteLine(" - Debugging set to INFORMATIVE");
            }

            Console.WriteLine(" - Is Live Server : " + (isLive ? "TRUE" : "FALSE"));
            Console.WriteLine(" - Maximum player count set to " + maxPlayers.ToString());
        }
    }
}
