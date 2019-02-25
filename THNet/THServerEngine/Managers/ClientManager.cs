using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENet;

namespace THServerEngine.Managers
{
    public class ClientManager
    {
        private Dictionary<uint, PlayerClient> _Clients;
        private Dictionary<uint, uint> NetID_To_Client;

        public ClientManager()
        {
            _Clients = new Dictionary<uint, PlayerClient>();
            NetID_To_Client = new Dictionary<uint, uint>();
        }

        public void AddClient(Peer peer, uint userID)
        {
            PlayerClient client = new PlayerClient(userID, peer.ID, peer);

            _Clients.Add(userID, client);
            NetID_To_Client.Add(peer.ID, userID);
        }

        #region Remove_Client

        public void RemoveClient(uint uID)
        {
            if (!_Clients.ContainsKey(uID))
            {
                return;
            }

            uint netID = _Clients[uID].ENetID;
            _Clients.Remove(uID);

            if (!NetID_To_Client.ContainsKey(netID))
            {
                return;
            }
            NetID_To_Client.Remove(netID);
        }

        public void RemoveClient_NetID(uint netID)
        {
            var client = GetClientFromNetID(netID);

            if(client != null)
            {
                _Clients.Remove(client.UserID);

                NetID_To_Client.Remove(netID);
            }
        }

        #endregion

        public PlayerClient GetClientFromNetID(uint netID)
        {
            if(!NetID_To_Client.ContainsKey(netID))
            {
                return null;
            }

            var uID = NetID_To_Client[netID];
            if(!_Clients.ContainsKey(uID))
            {
                return null;
            }

            return _Clients[uID];
        }
    }
}
