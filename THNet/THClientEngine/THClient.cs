using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ENet;

namespace THClientEngine
{
    public enum ConnectionStatus
    {
        DISCONNECTED,
        UNCONNECTED,
        CONNECTED
    }

    public class THNet_Client
    {
        private static Host _client;

        private IPEndPoint _ipep;
        private Peer _serverPeer;
        
        public delegate void DisconnectionEvent();
        public event DisconnectionEvent OnDisconnected;

        public static ConnectionStatus ConnectionStatus { get; private set; }

        public THNet_Client(IPEndPoint ipep, bool auto_run = false)
        {
            _ipep = ipep;

            Initialize();

            if (auto_run)
            {
                Start();
            }
        }

        /// <summary>
        /// Initialize Client 
        /// </summary>
        private void Initialize()
        {
            // Initialize Managers
            InitializeManagers();

            // Initialize ENet
            ENet.Library.Initialize();

            // Initialize Host
            _client = new Host();
        }

        /// <summary>
        /// Initialize Managers used by Client
        /// </summary>
        private void InitializeManagers()
        {

        }
        
        /// <summary>
        /// Start Client.
        /// </summary>
        public void Start()
        {
            Address address = new Address();

            address.SetHost(_ipep.Address.ToString());
            address.Port = (ushort)_ipep.Port;

            _client.Create();

            _serverPeer = _client.Connect(address);

            Event netEvent;

            Log.Write("Client Started.", LogType.SYSTEM);

            ConnectionStatus = ConnectionStatus.CONNECTED;

            while (ConnectionStatus == ConnectionStatus.CONNECTED)
            {
                bool polled = false;

                while (!polled)
                {
                    if (_client.CheckEvents(out netEvent) <= 0)
                    {
                        if (_client.Service(15, out netEvent) <= 0)
                            break;

                        polled = true;
                    }

                    switch (netEvent.Type)
                    {
                        case EventType.None:
                            break;

                        case EventType.Connect:
                            Log.Write("Client connected to server - ID: " + _serverPeer.ID, LogType.DEBUG);
                            break;

                        case EventType.Disconnect:
                            Log.Write("Client disconnected from server", LogType.DEBUG);

                            // Disconnected Event
                            OnDisconnected();

                            ConnectionStatus = ConnectionStatus.DISCONNECTED;
                            break;

                        case EventType.Timeout:
                            Log.Write("Client connection timeout", LogType.DEBUG);
                            break;

                        case EventType.Receive:
                            Log.Write("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length, LogType.VERBOSE);
                            netEvent.Packet.Dispose();
                            break;
                    }
                }

                if(Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        ManualDisconnect();
                    }
                }
            }

            _client.Flush();

        }

        /// <summary>
        /// Disconnect From Server
        /// </summary>
        public void ManualDisconnect()
        {
            ConnectionStatus = ConnectionStatus.DISCONNECTED;

            _serverPeer.DisconnectNow(0);
        }
    }
}
