using System;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections.Generic;
using MjClient.Logger;
using MjNetworkProtocol;
using System.Threading;

namespace MjClient.IO
{
    /// <summary>
    /// Handles Game Message by Tcp Connection.
    /// </summary>
    class ClientIO 
    {
        public event GetMessageHandler OnGetMessage;

        public delegate void GetMessageHandler(string message);

        string lineBreakMark = "\r\n";


        string ipAddress = "104.214.151.136";
        int port = 11600;
        TcpClient tcpClient;
        public ClientIO(){}


        public void MakeConnection()
        {
            tcpClient = new TcpClient(ipAddress, port);
            Thread thread = new Thread(RecieveMessage);
            thread.Start();
        }
        
        private void RecieveMessage()
        {
            StreamReader reader = new StreamReader(tcpClient.GetStream());
            string line = String.Empty;

            while (tcpClient.Connected)
            {
                line += reader.ReadLine();
                if (String.IsNullOrEmpty(line))
                {
                    break;
                }
                
                //delegate event
                OnGetMessage(line);
                line = String.Empty;
            } 
        }

        private void SendMessage(string message)
        {
            if (message == String.Empty)
            {
                return;
            }
            
            if (tcpClient.Connected)
            {
                message += lineBreakMark;
                Encoding enc = Encoding.UTF8;
                byte[] sendBytes = enc.GetBytes(message);
                //データを送信する
                tcpClient.GetStream().Write(sendBytes, 0, sendBytes.Length);
            }
        }

        public void SendMJsonObject(MJsonMessagePon jsonmsg)
        {
            SendMessage(JsonConvert.SerializeObject(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessageChi jsonmsg)
        {
            SendMessage(JsonConvert.SerializeObject(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessageDaiminkan jsonmsg)
        {
            SendMessage(JsonConvert.SerializeObject(jsonmsg));
        }
        public void SendMJsonObject(object jsonmsg)
        {
            SendMessage(JsonConvert.SerializeObject(jsonmsg));
        }


        //CtoS
        public void SendNone()
        {
            SendMessage(JsonConvert.SerializeObject(new MJsonMessageNone()));
        }
    }
}
