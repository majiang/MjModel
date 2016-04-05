using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjServer
{
    delegate void GetMessageFromClient(string message);
    interface ClientHolderInterface
    {
        event GetMessageFromClient OnGetMessageFromClient;
        void SendMessage(string message);
        void Disconnect();
    }
}
