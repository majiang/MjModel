using System;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections.Generic;
using MjClient.Logger;
using MjNetworkProtocol;

namespace MjClient.IO
{
    /// <summary>
    /// Handles Game Message by Tcp Connection.
    /// </summary>
    class ClientIO : System.Exception
    {
        public event GetMessageHandler OnGetMessage;

        public delegate void GetMessageHandler(string message);

        string lineBreakMark = "\r\n";


        string ipAddress = "127.0.0.1";
        int port = 11452;
        TcpClient tcpClient;

        public ClientIO ()
        {
            MakeConnection();
        }

        public void MakeConnection()
        {
            tcpClient = new TcpClient(ipAddress, port);
            RecieveMessage();
        }
        
        private async void RecieveMessage()
        {
            StreamReader reader = new StreamReader(tcpClient.GetStream());
            string line = String.Empty;

            while (tcpClient.Connected)
            {
                line += await reader.ReadLineAsync();
                if (String.IsNullOrEmpty(line))
                {
                    break;
                }
                else if (line.EndsWith(lineBreakMark) == false)
                {
                    continue;
                }

                //delegate event
                OnGetMessage(line);
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
