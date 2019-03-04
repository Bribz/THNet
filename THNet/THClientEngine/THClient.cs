using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        private ConcurrentQueue<int> _inboundEventQueue;
        private ConcurrentQueue<int> _outboundEventQueue;

        private static Host _client;

        private IPEndPoint _ipep;
        private Peer _serverPeer;

        private Thread _netThread;

        public delegate void DisconnectionEvent();
        public event DisconnectionEvent OnDisconnected;

        public static volatile ConnectionStatus ConnectionStatus;

        public THNet_Client(IPEndPoint ipep, bool auto_run = false)
        {
            _ipep = ipep;

            Initialize();

            if (auto_run)
            {
                Connect();
            }
        }

        /// <summary>
        /// Initialize Client 
        /// </summary>
        private void Initialize()
        {
            // Initialize Event Queue
            _inboundEventQueue = new ConcurrentQueue<int>();
            _outboundEventQueue = new ConcurrentQueue<int>();

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
            //Initialize Client Managers
        }

        public void Connect()
        {
            Address address = new Address();

            address.SetHost(_ipep.Address.ToString());
            address.Port = (ushort)_ipep.Port;

            _client.Create();

            _serverPeer = _client.Connect(address);
             
            Log.Write("Client Started.", LogType.SYSTEM);

            ConnectionStatus = ConnectionStatus.CONNECTED;

            _netThread = new Thread(Start);
            _netThread.Start();
        }
        
        /// <summary>
        /// Start Client.
        /// </summary>
        private void Start()
        {
            Event netEvent;

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

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        ManualDisconnect();
                    }
                }
            }

            _serverPeer.DisconnectNow(0);

            Log.Write("Client closed connection to Server", LogType.SYSTEM);

            _client.Flush();

            /*
            if (_netThread.IsAlive)
            {
                _netThread.Abort();
            }
            */
        }

        /// <summary>
        /// Disconnect From Server
        /// </summary>
        public void ManualDisconnect()
        {
            ConnectionStatus = ConnectionStatus.DISCONNECTED;
            
            //_netThread.Abort();

            _serverPeer.DisconnectNow(0);
        }
    }
}
