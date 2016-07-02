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
using System.Diagnostics;
namespace MjServer
{

    class ClientHolderUsingTcpHolder : IClientHolder
    {
        TcpClient tcpClient;
        public event GetMessageFromClient OnGetMessageFromClient;
        public event ConnectionBroken ConnectionBrokenHandler;
        //public event OverResponceTimeLimit OnOverResponceTimeLimit;
        //public event OverWaitingStartGameTimeLimit OnOverWaitingStartGameTimeLimit;
        

        public ClientHolderUsingTcpHolder(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public async Task StartWaiting()
        {
            //SendHello
            var helloMessage = JsonConvert.SerializeObject(new MJsonMessageHello());
            SendMessageToClient(helloMessage);

            Debug.WriteLine(
                "Connect to Client ({0}:{1})" ,
                ( (IPEndPoint)tcpClient.Client.RemoteEndPoint ).Address ,
                ( (IPEndPoint)tcpClient.Client.RemoteEndPoint ).Port
                );

            try
            {
                NetworkStream stream = tcpClient.GetStream();
                StreamReader reader = new StreamReader(stream);

                while (tcpClient.Connected)
                {
                    string line = await reader.ReadLineAsync() + NetworkConstants.NewLineString;
                    //Debug.WriteLine("get:"+ line);
                    GetMessageFromClient(line);
                }
            }
            catch (IOException e)
            {
                Debug.Fail("error! : "+ e.Message);
                Disconnect();
                ConnectionBrokenHandler();
            }

        }

        public void GetMessageFromClient(string message)
        {
            OnGetMessageFromClient(message, this);
        }

        public void Disconnect()
        {
           tcpClient.Close();
        }

        public void SendMessageToClient(string message)
        {
            message += NetworkConstants.NewLineString;
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                //Debug.WriteLine("send:"+ message);
            }
            catch(IOException e)
            {
                Debug.Fail("error! :"+ e.Message);
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
