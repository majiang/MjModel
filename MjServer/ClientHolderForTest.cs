﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MjNetworkProtocolLibrary;
using Newtonsoft.Json;

namespace MjServer
{
    public class ClientHolderForTest : IClientHolder
    {
        public event ConnectionBroken ConnectionBrokenHandler;
        public event GetMessageFromClient OnGetMessageFromClient;

        public List<MJsonMessageAll> ReceivedMessageList = new List<MJsonMessageAll>();
        

        public void Disconnect()
        {
            Debug.WriteLine("Disconnect");    
        }

        public void ResetResponceTimeCount()
        {

        }

        public void ResetWaitingTimeCountForStartGame()
        {
            throw new NotImplementedException();
        }

        public void SendMessageToClient(string message)
        {
            ReceivedMessageList.Add(JsonConvert.DeserializeObject<MJsonMessageAll>(message));
        }

        public Task StartWaiting()
        {
            throw new NotImplementedException();
        }

        public void GetMessageFromClient(string message)
        {
            OnGetMessageFromClient(message, this);
        }


    }
}
