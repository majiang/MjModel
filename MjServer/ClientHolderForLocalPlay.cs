using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjNetworkProtocolLibrary;

namespace MjServer
{
    public class ClientHolderForLocalPlay : IClientHolder
    {
        public event ConnectionBroken ConnectionBrokenHandler;
        public event GetMessageFromClient OnGetMessageFromClient;

        public IServerHolder ClientSideServerHolder;

        List<string> getMessageList = new List<string>();

        public void Disconnect()
        {
            
        }

        public void ResetResponceTimeCount()
        {
            throw new NotImplementedException();
        }

        public void ResetWaitingTimeCountForStartGame()
        {
            throw new NotImplementedException();
        }

        public void GetMessageFromClient(string message)
        {
            getMessageList.Add(message);
        }

        public void ProcessMessage()
        {
            getMessageList.ForEach(e => OnGetMessageFromClient(e,this));
            getMessageList.Clear();
        }

        public Task StartWaiting()
        {
            throw new NotImplementedException();
        }

        public void SendMessageToClient(string message)
        {
            ClientSideServerHolder.GetMessageFromServer(message);
        }

        public void Reset()
        {
            getMessageList.Clear();
        }
    }
}
