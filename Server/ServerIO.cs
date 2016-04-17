using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class ServerIO
    {
        public delegate bool GetMessageHandler();

        public event GetMessageHandler OnGetMessage;

        static void StartServer()
        {
            Console.WriteLine("START");


            System.Net.IPAddress ipAdd = IPAddress.Loopback;//LAN
            //System.Net.IPAddress ipAdd = IPAddress.Any;//WAN

            int port = 11600;
            TcpListener server = new TcpListener(ipAdd, port);

            server.Start();
            Console.WriteLine("Start Listen({0}:{1})",
                ((System.Net.IPEndPoint)server.LocalEndpoint).Address,
                ((System.Net.IPEndPoint)server.LocalEndpoint).Port);

            List<TcpClient> clientList = new List<TcpClient>();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clientList.Add(client);
                Task.Run(() => GameStream(client));
            }

        }

        static async Task GameStream(TcpClient client)
        {
            Console.WriteLine("Connect to Client ({0}:{1})",
                ((IPEndPoint)client.Client.RemoteEndPoint).Address,

                ((IPEndPoint)client.Client.RemoteEndPoint).Port);

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);

            while (client.Connected)
            {
                string line = await reader.ReadLineAsync();


                OnGetMessage();
            }

        }




    }
}
