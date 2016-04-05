using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjServer
{
    delegate void GetMessageFromClient(string message);
    delegate void ConnectionBroken();
    interface ClientHolderInterface
    {
        event GetMessageFromClient OnGetMessageFromClient;
        event ConnectionBroken OnConnectionBroken;
        Task StartWaiting();
        void SendMessage(string message);
        void Disconnect();
    }
}
