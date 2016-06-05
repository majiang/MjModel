using System;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections.Generic;
using MjClient.Logger;
using MjNetworkProtocolLibrary;
using System.Threading;
using System.Diagnostics;

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


        string ipAddress = "127.0.0.1";
        int port = 11601;
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
            try
            {
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
            catch (Exception e)
            {
                Debug.Fail("error! : " + e.Message);
            }
        }

        private void SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            //Debug.Assert(string.IsNullOrEmpty(message));
            try
            {
                if (tcpClient.Connected)
                {
                    message += lineBreakMark;
                    Encoding enc = Encoding.UTF8;
                    byte[] sendBytes = enc.GetBytes(message);
                    tcpClient.GetStream().Write(sendBytes, 0, sendBytes.Length);
                }
            }
            catch (Exception e)
            {
                Debug.Fail("error! : " + e.Message);
            }
        }

        private string MjsonObjectToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public void SendMJsonObject(MJsonMessageJoin jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessagePon jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessageChi jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessageDaiminkan jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessageNone jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }

        public void SendMJsonObject(MJsonMessageHora jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessageDahai jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessageAnkan jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessageKakan jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }
        public void SendMJsonObject(MJsonMessageReach jsonmsg)
        {
            SendMessage(MjsonObjectToString(jsonmsg));
        }

    }
}
