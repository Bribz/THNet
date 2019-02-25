using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENet;
using THServerEngine.Managers;

namespace THServerEngine
{
    public enum ServerStatus
    {
        OFF,
        RUNNING,
        STOPPING
    }

    public class THNet_Server
    {
        // Managers
        private ConnectionApprovalManager m_ConnectionApprovalManager;
        private ClientManager m_ClientManager;

        // Server
        private Host _server;

        // Server Configs
        private ServerConfigs _configs;

        private DateTime _StartTime;

        private ServerStatus _status;

        public THNet_Server(int max_clients = 16, bool isLive = false, bool auto_run = false)
        {
            Initialize(max_clients, isLive);

            if(auto_run)
            {
                Run();
            }
        }

        /// <summary>
        /// Set up ENet Server prior to launch. Will set up necessary managers as well.
        /// </summary>
        private void Initialize(int max_clients = 16, bool isLive = false)
        {
            Log.Write("Initializing Server Configs...", LogType.SYSTEM);
            _configs = new ServerConfigs()
            {
                MAX_CLIENTS = max_clients,
                PORT = 7735
            };

            // Initialize Server Managers
            InitializeManagers();

            ENet.Library.Initialize();

            _status = ServerStatus.OFF;
        }

        /// <summary>
        /// Set up relevant server managers
        /// </summary>
        private void InitializeManagers()
        {
            Log.Write("Initializing Managers...", LogType.SYSTEM);

            // Initialize ConnectionApprovalManager
            m_ConnectionApprovalManager = new ConnectionApprovalManager();

            // Initialize ClientManager
            m_ClientManager = new ClientManager();
        }

        public void Run()
        {
            Log.Write("Starting Server...", LogType.SYSTEM);

            // Setup address for ENet Host
            Address address = new Address();
            address.Port = _configs.PORT;

            // Create ENet Host
            _server = new Host();
            
            _server.Create(address, _configs.MAX_CLIENTS);

            _status = ServerStatus.RUNNING;

            _StartTime = DateTime.Now;
            Log.Write($"Server Started : {_StartTime.ToShortDateString()} - {_StartTime.ToShortTimeString()}\n\n", LogType.SYSTEM);
            
            Event netEvent;
            
            while (_status == ServerStatus.RUNNING)
            {
                // Check Inputs from Console
                CheckServerInput();

                bool polled = false;

                while (!polled)
                {
                    if (_server.CheckEvents(out netEvent) <= 0)
                    {
                        if (_server.Service(15, out netEvent) <= 0)
                            break;

                        polled = true;
                    }

                    switch (netEvent.Type)
                    {
                        case EventType.None:
                            break;
                        case EventType.Connect:

                            var errCode = m_ConnectionApprovalManager.ApproveConnection(null, netEvent.Peer.IP);
                            if (errCode == ConnectionApprovalCode.APPROVED)
                            {
                                Log.Write($"Client connected - ID:  {netEvent.Peer.ID}, IP: {netEvent.Peer.IP}", LogType.DEBUG);

                                m_ClientManager.AddClient(netEvent.Peer, 0);
                            }
                            else
                            {
                                // Handle Error
                                Log.Write($"Denied connection: Reason:{m_ConnectionApprovalManager.GetDenyReason(errCode)}, IP: {netEvent.Peer.IP}", LogType.DEBUG);

                                netEvent.Peer.DisconnectNow((uint)errCode);
                            }
                            break;

                        case EventType.Disconnect:
                            Log.Write("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP, LogType.DEBUG);

                            m_ClientManager.RemoveClient_NetID(netEvent.Peer.ID);
                            break;

                        case EventType.Timeout:
                            Log.Write("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP, LogType.DEBUG);

                            m_ClientManager.RemoveClient_NetID(netEvent.Peer.ID);
                            break;

                        case EventType.Receive:
                            Log.Write("Packet received from - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP + ", Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length, LogType.VERBOSE);
                            netEvent.Packet.Dispose();
                            break;
                    }
                }
            }

            Log.Write("Flushing Server...", LogType.SYSTEM);

            _server.Flush();
            
            Log.Write("Server shut down successfully.", LogType.SYSTEM);
            Log.Write($"Run Time : {Util.Time.GetRunTimeOffset(_StartTime, DateTime.Now)}\n", LogType.SYSTEM);

            Console.ReadKey();
        }

        private void CheckServerInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                //Quit requested
                if (key.Key == ConsoleKey.Escape)
                {
                    _status = ServerStatus.STOPPING;

                    Log.Write("Server shutdown requested...", LogType.SYSTEM);
                }
            }
        }
    }
}
