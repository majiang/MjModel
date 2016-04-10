using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjServer
{
    delegate void GetMessageFromClient(string message, ClientHolderInterface clientHolder);
    delegate void ConnectionBroken();
    delegate void OverResponceTimeLimit();
    delegate void OverWaitingStartGameTimeLimit();

    interface ClientHolderInterface
    {
        event GetMessageFromClient GetMessageFromClientHandler;
        event ConnectionBroken ConnectionBrokenHandler;
        Task StartWaiting();
        void SendMessage(string message);
        void Disconnect();
        void ResetResponceTimeCount();
        void ResetWaitingTimeCountForStartGame();
    }
}
