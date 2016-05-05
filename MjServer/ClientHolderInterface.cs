using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjServer
{
    public delegate void GetMessageFromClient(string message, ClientHolderInterface clientHolder);
    public delegate void ConnectionBroken();
    public delegate void OverResponceTimeLimit();
    public delegate void OverWaitingStartGameTimeLimit();

    public interface ClientHolderInterface
    {
        event GetMessageFromClient GetMessageFromClientHandler;
        event ConnectionBroken ConnectionBrokenHandler;
        Task StartWaiting();
        void SendMessageToClient(string message);
        void Disconnect();
        void ResetResponceTimeCount();
        void ResetWaitingTimeCountForStartGame();
    }
}
