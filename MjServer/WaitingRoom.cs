using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MjServer
{
    delegate void StartRoom(
        ClientHolderInterface client0,
        ClientHolderInterface client1,
        ClientHolderInterface client2,
        ClientHolderInterface client3
        );

    class WaitingRoom
    {
        public event StartRoom OnStartRoom;
        List<ClientHolderInterface> waitingClientHolderList;
        public WaitingRoom() { }
        


        public void StartWaiting()
        {

            System.Net.IPAddress ipAdd = IPAddress.Parse(Properties.Settings.Default.ipAddress);//LAN
            int port = Properties.Settings.Default.port;
            TcpListener server = new TcpListener(ipAdd, port);

            server.Start();
            Console.WriteLine("Start Listen({0}:{1})",
                ((System.Net.IPEndPoint)server.LocalEndpoint).Address,
                ((System.Net.IPEndPoint)server.LocalEndpoint).Port);

            waitingClientHolderList = new List<ClientHolderInterface>();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ClientUsingTcpHolder clientHolder = new ClientUsingTcpHolder(client);
                waitingClientHolderList.Add(clientHolder);
                Task.Run(() => clientHolder.StartWaiting());
            }
        }

        public void CheckWaitingTime()
        {

        }

    }
}
