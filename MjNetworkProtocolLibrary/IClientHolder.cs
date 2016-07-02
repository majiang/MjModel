using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjNetworkProtocolLibrary
{
    public delegate void GetMessageFromClient(string message, IClientHolder clientHolder);
    public delegate void ConnectionBroken();
    public delegate void OverResponceTimeLimit();
    public delegate void OverWaitingStartGameTimeLimit();

    // save client infomation on server side 
    public interface IClientHolder
    {
        event GetMessageFromClient OnGetMessageFromClient;
        event ConnectionBroken ConnectionBrokenHandler;
        Task StartWaiting();
        void GetMessageFromClient(string message);
        void SendMessageToClient(string message);
        void Disconnect();
        void ResetResponceTimeCount();
        void ResetWaitingTimeCountForStartGame();
    }
}
