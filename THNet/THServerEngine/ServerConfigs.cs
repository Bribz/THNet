using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace THServerEngine
{
    public struct ServerConfigs
    {
        public IPAddress EXTERNAL_IP;
        public ushort PORT;
        public int MAX_CLIENTS;
        public bool LIVE;
    }
}
