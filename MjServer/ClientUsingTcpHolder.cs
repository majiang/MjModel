using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace MjServer
{

    class ClientUsingTcpHolder : ClientHolderInterface
    {
        TcpClient tcpClient;
        public ClientUsingTcpHolder(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public async Task StartWaiting()
        {
            //SendHello

            Console.WriteLine("Connect to Client ({0}:{1})",
                ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address,
                ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Port);

            NetworkStream stream = tcpClient.GetStream();
            StreamReader reader = new StreamReader(stream);

            while (tcpClient.Connected)
            {
                string line = await reader.ReadLineAsync() + '\n';
                OnGetMessageFromClient(line);
            }
        }

        public event GetMessageFromClient OnGetMessageFromClient;
        public event ConnectionBroken OnConnectionBroken;
        public event OverResponceTimeLimit OnOverResponceTimeLimit;
        public event OverWaitingStartGameTimeLimit OnOverWaitingStartGameTimeLimit;

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            //message +=
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
            catch(Exception e)
            {
                Console.WriteLine("error! : {0}", e.Message);
                OnConnectionBroken();
            }
        }

        public void StartCountResponceTime()
        {
            throw new NotImplementedException();
        }


        public void StartCountWaitingStartGameTime()
        {
            throw new NotImplementedException();
        }


    }
}
