using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MjServer
{
    class WaitingRoom
    {
        List<ClientHolderInterface> clientHolderList;
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

            clientHolderList = new List<ClientHolderInterface>();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ClientUsingTcpHolder clientHolder = new ClientUsingTcpHolder(client);
                clientHolderList.Add(clientHolder);
                
                Task.Run(() => clientHolder.StartWaiting());
            }
        }


        public void RegisterClient(ClientUsingTcpHolder client)
        {

        }
    }
}
