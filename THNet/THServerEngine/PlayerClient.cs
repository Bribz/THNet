using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENet;

namespace THServerEngine
{
    public class PlayerClient
    {
        public Peer client_peer;

        public uint UserID;

        public uint ENetID;

        public string Username;

        public PlayerClient(uint userID, uint netID, Peer netPeer)
        {
            client_peer = netPeer;
            UserID = userID;
            ENetID = netID;
        }

        public void SetClientData(string Username)
        {

        }
    }
}
