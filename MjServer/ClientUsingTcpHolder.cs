using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using MjNetworkProtocolLibrary;
using Newtonsoft.Json;

namespace MjServer
{

    class ClientUsingTcpHolder : ClientHolderInterface
    {
        TcpClient tcpClient;
        public event GetMessageFromClient GetMessageFromClientHandler;
        public event ConnectionBroken ConnectionBrokenHandler;
        //public event OverResponceTimeLimit OnOverResponceTimeLimit;
        //public event OverWaitingStartGameTimeLimit OnOverWaitingStartGameTimeLimit;
        

        public ClientUsingTcpHolder(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public async Task StartWaiting()
        {
            //SendHello
            var helloMessage = JsonConvert.SerializeObject(new MJsonMessageHello());
            SendMessage(helloMessage);
            
            Console.WriteLine(
                "Connect to Client ({0}:{1})",
                ( (IPEndPoint)tcpClient.Client.RemoteEndPoint ).Address,
                ( (IPEndPoint)tcpClient.Client.RemoteEndPoint ).Port
                );
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                StreamReader reader = new StreamReader(stream);
                while (tcpClient.Connected)
                {
                    string line = await reader.ReadLineAsync() + NetworkConstants.NewLineString;
                    GetMessageFromClientHandler(line, this);
                }
            }
            catch(Exception e)
            {
                Disconnect();
                ConnectionBrokenHandler();
            }
        }
        
        public void Disconnect()
        {
           tcpClient.Close();
        }

        public void SendMessage(string message)
        {
            message += NetworkConstants.NewLineString;
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
            catch(Exception e)
            {
                Console.WriteLine("error! : {0}", e.Message);
                ConnectionBrokenHandler();
            }
        }

        DateTime responceTime;
        public void ResetResponceTimeCount()
        {
            responceTime = DateTime.Now;
        }

        DateTime waitingTimeForStartGame;
        public void ResetWaitingTimeCountForStartGame()
        {
            waitingTimeForStartGame = DateTime.Now;
        }


    }
}
