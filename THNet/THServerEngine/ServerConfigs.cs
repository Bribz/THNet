using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace THServerEngine
{
    public class ServerConfigs
    {
        public IPAddress EXTERNAL_IP;
        public ushort PORT = 7735;
        public int MAX_CLIENTS = 64;
        public bool LIVE = false;

        public bool AUTHORITATIVE = true;
    }
}
